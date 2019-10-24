import pika

class BasicRMQClient:
	"""A very basic RabbitMQ client handling connection failures"""
	def __init__(self, server, port, user, password, virtual_host='/'):
		self.server = server
		self.port = port
		self.user = user
		self.password = password
		self.virtual_host = virtual_host

	# publishes a message to the specified exchange on the active channel
	def publish_exchange(self, channel, exchange, body, routing_key=''):
		channel.basic_publish(exchange=exchange, body=body, routing_key=routing_key)

	# processes message from RabbitMQ
	def process(self, callback_on_message, source_queue, completed_exchange):
		# define our connection parameters
		creds = pika.PlainCredentials(self.user, self.password)
		connection_params = pika.ConnectionParameters(host=self.server,
		                                  port=self.port,
		                                  virtual_host=self.virtual_host,
		                                  credentials=creds)
		# Connect to RMQ and wait until a message is received
		while(True):
			try:
				print("Connecting to %s" % self.server)
				self.connection = pika.BlockingConnection(connection_params)

				# create channel and a queue bound to the source exchange 
				self.channel = self.connection.channel()

				self.channel.basic_consume(
				    queue=source_queue, on_message_callback=callback_on_message, auto_ack=True)

				print(' [*] Waiting for messages. To exit press CTRL+C')
				try:
					self.channel.start_consuming()
				except KeyboardInterrupt:
					self.channel.stop_consuming()
					self.connection.close()
					break
			# Recover from server-initiated connection closure - handles manual RMQ restarts
			except pika.exceptions.ConnectionClosedByBroker:
				continue
		    # Do not recover on channel errors
			except pika.exceptions.AMQPChannelError as err:
				print("Channel error: {}, stopping...".format(err))
				break
		    # Recover on all other connection errors
			except pika.exceptions.AMQPConnectionError:
				print("Connection was closed, retrying...")
				continue

# EXAMPLE USAGE:
# Processes the frame here and publish the result on an exchange
#def callback_on_message(ch, method, properties, body):
#	print(" [x] Received %r" % body)
#	# do work here on the message
#	print("Publishing completed message to {}".format(rmq_completed_exchange))
#	rmq_client.publish_exchange(ch, rmq_completed_exchange, body)

# Create RMQ client
#rmq_client = BasicRMQClient(rmq_server, rmq_port, rmq_user, rmq_password, rmq_virtual_host)
# start processing messages from the rmq_source_queue
#rmq_client.process(callback_on_message, rmq_source_queue, rmq_completed_exchange)