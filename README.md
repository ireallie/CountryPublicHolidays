# CountryPublicHolidays

A small WEB API application that returns country public holidays.

## Page Contents
Deployment Procedure
Build the Docker Image locally
Pushing the image to docker hub using ```docker push``` command
Instructing Azure to pull the image from Docker Hub
Enabling Continuous Deployment 

#### Deployment instructions

1. Building a docker image of the app in your local docker environment.
2. Pushing the image to your repository on docker hub.
3. Instructing Azure to pull the image from the docker hub and make the app live.

![image](https://user-images.githubusercontent.com/46068169/172868857-921e16c8-e83f-4f54-9a08-2f97f437ad49.png)

#### Build the Docker Image locally

Open the command prompt and navigate to the directory of the Dockerfile. Build the image so that it is named in this format – ```username/repository:tag```.

Here ```username``` is the name of your docker hub account and ```repository``` is the name of a repository in your docker hub account. The ```tag``` can be anything like ```latest```, etc.

The docker build command to run is:

```docker build -t username/repository:tag```

#### Pushing the image to docker hub using “docker push” command

Now login to your docker hub account and create a new repository by the name of ```repository```. Once the repository is created it will show the push command that will push a local image to it. 
The command is:
	
```docker push username/repository:tag```

Pushing an image to docker hub requires an authentication. That is, you should be logged on to your docker hub account in your docker desktop. Right click on docker desktop and you can find out if you are logged on or not. See below image:
Suppose you are not logged on then in that case you can log on from your docker desktop itself.

Another way to do is through login from the command prompt window. The command to login is:

```docker login```

It will ask you for your username and password for the docker hub account. Note that when typing the password, it will not show there. You will get login successful message. Check the below image which shown this message on successful login.

Next step is to push the image. The command to run is:

```docker push username/repository:tag```

In a few minutes time, the image will be pushed to docker hub. Now open your repository in the docker hub and you will find this pushed image there.

#### Instructing Azure to pull the image from Docker Hub

1. Login to your Azure account and go to App Services, then click Create.
2. Create Web App screen, give the name to your app. For the Publish field select Docker Container.
3. Select the Operating System to be either Linux or Windows. Then click the Next: Docker > button.
4. In the next screen you have to do some Docker settings for your app. Select Single Container as our app is based on single container. For the Image Source select Docker Hub, and for the Image and tag field add ```username/repository:tag``` same as in your docker hub.
5. Click the Review + create button. The screen will show your docker setting for review. Next click the Create button to create your app and make it live.

Azure will start the deployment of your app and it will be done in a minute or two. Azure will notify you once it is complete.

After that go your app service where you will see the url of this app.

Click the url to open the app running in the browser.

#### Continuous Deployment

Go to the Deployment Center setting and toggle the Continuous deployment option to On and save it.

Next, rebuild the image:

```docker build -t username/repository:tag```

Push the image to docker hub:
	
```docker push username/repository:tag```

Azure will automatically detect when the image is updated in Docker Hub and it will update the app with the new image.

Now open the apps URL once again and you will notice the changes.


