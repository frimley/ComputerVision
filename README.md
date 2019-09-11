# ComputerVision

Repository contains:
- WinForms application to receive and send video frames from a webcam to RabbitMQ
- .Net Core frame aggregator to receive data from RabbitMQ from distributed microservices and draw on video frames
- Example Python face detection model configured to be a Docker container
- RabbitMQ helper libary to connect to RabbitMQ (RMQ.py)

Sample usage of RMQ.py:
```python
import RMQ

rmq_server = "localhost"
rmq_port = 5672
rmq_user = "rmq_user"
rmq_password = "rmq_user_password"
rmq_virtual_host = "/"
rmq_source_queue = "source_queue"
rmq_completed_exchange = "processed_exchange"

# Process the message here in this function and publish the result on the exchange
def callback_on_message(ch, method, properties, body):
    print(" [x] Received %r" % body)
    
    # do work here on the message
    # calculate or process the message
    # example, return the length of the string
    number_of_characters = len(body)
    
    print("Publishing completed message to {}".format(rmq_completed_exchange))
    rmq_client.publish_exchange(ch, rmq_completed_exchange, number_of_characters)

# Create RMQ client
rmq_client = BasicRMQClient(rmq_server, rmq_port, rmq_user, rmq_password, rmq_virtual_host)
# start processing messages from the rmq_source_queue
rmq_client.process(callback_on_message, rmq_source_queue, rmq_completed_exchange)
```
