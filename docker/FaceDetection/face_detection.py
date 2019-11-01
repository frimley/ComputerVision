import cv2
import face_recognition
import os
import io
import argparse
import numpy as np
import pysmile
import base64
import datetime
from RMQ import BasicRMQClient

# Get config/settings for RMQ  from either passed arguments, environment variables (Docker) or defaults
parser = argparse.ArgumentParser(description='Detect faces in an image')
parser.add_argument('--rmq-server', help='RMQ server')
parser.add_argument('--rmq-port', help='RMQ server port')
parser.add_argument('--rmq-user', help='RMQ username')
parser.add_argument('--rmq-password', help='RMQ password')
parser.add_argument('--rmq-virtual-host', help='RMQ virtual host')
parser.add_argument('--rmq-source-queue', help='RMQ source queue')
parser.add_argument('--rmq-completed-exchange', help='RMQ completed exchange')
args = parser.parse_args()

rmq_server = args.rmq_server or os.getenv('rmq-server') or '192.168.0.29'
rmq_port = args.rmq_port or os.getenv('rmq-port') or 5672
rmq_user = args.rmq_user or os.getenv('rmq-user') or 'computer_vision'
rmq_password = args.rmq_password or os.getenv('rmq-password') or 'S33Th1ngs'
rmq_virtual_host = args.rmq_virtual_host or os.getenv('rmq-virtual-host') or '/'
rmq_source_queue = args.rmq_source_queue or os.getenv('rmq-source-queue') or 'queue.frames.source.face-detection'
rmq_completed_exchange = args.rmq_completed_exchange or os.getenv('rmq-completed-exchange') or 'exchange.object.face.source'
# end config/settings

# Processes the frame here and publish the result on an exchange
def callback_on_message(ch, method, properties, body):
	time_start = datetime.datetime.now()

	# byte array to bitmap
	frame = cv2.imdecode(np.asarray(bytearray(body)), cv2.IMREAD_COLOR)

	# Resize frame of video to 1/4 size for faster face recognition processing
	#small_frame = cv2.resize(frame, (0, 0), fx=0.25, fy=0.25)
	small_frame = frame # no resizing
	#cv2.imwrite('test.bmp', small_frame)

	# Convert the image from BGR color (which OpenCV uses) to RGB color (which face_recognition uses)
	rgb_small_frame = small_frame[:, :, ::-1]

	# Find all the faces and face encodings in the current frame of video
	face_locations = face_recognition.face_locations(rgb_small_frame)

	# Display the results
	for (top, right, bottom, left) in face_locations:
		# Scale back up face locations since the frame we detected in was scaled to 1/4 size
#		top *= 4
#		right *= 4
#		bottom *= 4
#		left *= 4

		# encode face img for transport
		retval, buffer = cv2.imencode('.bmp', frame[top:bottom, left:right].copy())

		# build a message to send to RMQ 
		message = {
			'top': top,
			'right': right,
			'bottom': bottom,
			'left': left,
			'process_time': (datetime.datetime.now()-time_start).total_seconds(),
			'face': base64.b64encode(buffer) #extract the face
		}

		print('Processed %s' % message['process_time'])

		# push the faces to RMQ - serialize message with smile binary format
		rmq_client.publish_exchange(ch, rmq_completed_exchange, pysmile.encode(message))

# Create RMQ client
rmq_client = BasicRMQClient(rmq_server, rmq_port, rmq_user, rmq_password, rmq_virtual_host)
# start processing messages from the rmq_source_queue
rmq_client.process(callback_on_message, rmq_source_queue, rmq_completed_exchange)