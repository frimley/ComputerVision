import pika
import json
import random
from RMQ import BasicRMQClient

rmq_server = '192.168.56.1'
rmq_port = 5672
rmq_user = 'model_user'
rmq_password = 'm0d3l***'
rmq_virtual_host = '/'
rmq_source_queue = 'queue.model.input'
rmq_completed_exchange = 'exchange.model.output'

#######################
# Initialize your model here
#######################

# currently there's nothing to initialize, but this is where you'd instantiate your model, load weights etc

# #######################
# This is a special function that gets called when a message is received on queue.model.input
# Add your model processing code here
# #######################
def callback_on_message_received(ch, method, properties, body):
	print('Message received %s' % body)

	# deserialize the json string into an object
	params = json.loads(body)

	# access the json params as a normal object once it has been deserialized
	print('Parameters: %s, %s, %s' % (params['param1'], params['param2'], params['param3']))

	# perform model calculation here (choosing random value as an example)
	output_value = random.randint(1, 5)

	# what JSON will this model return?
	return_json = '{category: %s}' % output_value

	# Send the return JSON to RabbitMQ exchange exchange.model.output
	rmq_client.publish_exchange(ch, rmq_completed_exchange, return_json)

#######################
# Program starts here:
#######################
# Create RMQ client
rmq_client = BasicRMQClient(rmq_server, rmq_port, rmq_user, rmq_password, rmq_virtual_host)

# Start processing messages from the rmq_source_queue - this blocks
rmq_client.process(callback_on_message_received, rmq_source_queue, rmq_completed_exchange)