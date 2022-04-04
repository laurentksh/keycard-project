#!/bin/bash

cd /home/pi/keycardiot

rm -rf ./app
rm master.zip

wget -P ./ https://github.com/laurentksh/keycard-project/archive/refs/heads/master.zip
unzip ./master.zip -d ./app

cd ./app/keycard-project-master

docker compose -f docker-compose.rasp.yml up -d