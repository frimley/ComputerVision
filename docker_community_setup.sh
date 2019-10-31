#############
# Install Docker Community Edition
#############

sudo apt update

# remove any old versions of Docker
sudo apt remove docker docker-engine docker.io

# install Docker
sudo apt install -y docker.io

# setup to start at system startup
sudo systemctl start docker
sudo systemctl enable docker
