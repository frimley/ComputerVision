# Use an official Python runtime
FROM python:2.7

# Set the working directory to /app
WORKDIR /app

# Copy the current directory contents into an app directory within the container /app
COPY . /app

# Install any needed packages specified in requirements.txt
RUN pip install --trusted-host pypi.python.org -r requirements.txt

# Run model.py when the container launches
CMD ["python", "model.py"]