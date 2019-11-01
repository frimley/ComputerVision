
# Use an official Python runtime as a parent image
FROM python:2.7

# Set the working directory to /app
WORKDIR /app

# Copy the current directory contents into the container at /app
COPY . /app

# Need cmake to compile dlib
RUN apt -y update
RUN apt install -y build-essential cmake pkg-config
RUN apt install -y libx11-dev libatlas-base-dev
RUN apt install -y libgtk-3-dev libboost-python-dev

ENV rmq-server 192.168.0.29
ENV rmq-source-exchange queue.frames.source.face-detection
ENV rmq-completed-exchange exchange.object.face.source

# Install any needed packages specified in requirements.txt
RUN pip install --trusted-host pypi.python.org -r requirements.txt

# Run app.py when the container launches
CMD ["python", "face_detection.py"]