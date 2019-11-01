#############
# PART 1: Install the latest Erlang 
#############

# Import the Erlang repo key
wget -O- https://packages.erlang-solutions.com/ubuntu/erlang_solutions.asc | sudo apt-key add -

# Add Erlang repository
echo "deb https://packages.erlang-solutions.com/ubuntu bionic contrib" | sudo tee /etc/apt/sources.list.d/rabbitmq.list

# Install Erlang
sudo apt update
sudo apt -y install erlang erlang-nox

#############
# PART 2: Install RabbitMQ
#############

# Add RabbitMQ repo keys
wget -O- https://dl.bintray.com/rabbitmq/Keys/rabbitmq-release-signing-key.asc | sudo apt-key add -
wget -O- https://www.rabbitmq.com/rabbitmq-release-signing-key.asc | sudo apt-key add -

# Add RabbitMQ repository
echo "deb https://dl.bintray.com/rabbitmq/debian $(lsb_release -sc) main" | sudo tee /etc/apt/sources.list.d/rabbitmq.list

# Install RabbitMQ server
sudo apt -y install rabbitmq-server

# If the erlang version isn't late enough because apt install didn't find it,
# you can download and install erlang manually from (but first remove the current version):
# sudo apt -y remove erlang erlang-nox 
# Download manually from here:
# https://www.erlang-solutions.com/resources/download.html

#############
# PART 2: Configure the RabbitMQ instance
#############

sudo service rabbitmq-server start
# Enable the web plugin for RabbitMQ management
sudo rabbitmq-plugins enable rabbitmq_management
sudo service rabbitmq-server restart
# Visit web admin at: http://localhost:15672