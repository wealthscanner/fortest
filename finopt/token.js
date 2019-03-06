const path = require('path');
const fs = require('fs');

var jwt = require('jsonwebtoken');
var privateKey = fs.readFileSync("/home/martin/Dev/fortest/finopt/privateKey.txt", "utf8");  // Location of the file with your private key
var payload = {};
var currentTime =  Math.floor(Date.now() / 1000);
var signOptions = {
	algorithm: "RS512"  // Yodlee requires RS512 algorithm when encoding JWT
};

payload.iss = "c82579bd-7217-43c7-a6c5-fa9a75746b9a"; // The issuer id from the API Dashboard
payload.iat = currentTime;  // Epoch time when token is issued in seconds
payload.exp = currentTime + 900;  // Epoch time when token is set to expire. Must be 900 seconds
payload.sub = "sbMem5c73e905ecbda1";

var token = jwt.sign(payload, privateKey, signOptions);
console.log(token);

