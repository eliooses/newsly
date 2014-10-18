var restify = require('restify');
var cheerio = require('cheerio');
var mongoose = require('mongoose');
var request = require('request');

function respond(req, res, next) {
  res.send({'name' : req.params.name});
  next();
}




function test(req, res){

	var main = "<figure class=\"element element-image\" data-media-id=\"gu-fc-5a0eac9a-d320-4659-8275-045e8e142d06\"> <img src=\"http://static.guim.co.uk/sys-images/Guardian/Pix/pictures/2014/10/18/1413635942356/Arsene-Wenger-010.jpg\" alt=\"Arsene Wenger\" width=\"460\" height=\"276\" class=\"gu-image\" /> <figcaption> <span class=\"element-image__caption\">Arsene Wenger’s early years at Arsenal brought huge success but trophies have been more elusive recently. Photograph: Tom Jenkins for the Guardian</span> </figcaption> </figure>";
	res.send({"image":extractImageURI(main)});
}

function getAllNews(req,res){
	var data = request('http://content.guardianapis.com/search?api-key=t3myqd7scnfu4t5w8zp7jx4v&show-fields=headline,trailText&page-size=100', function (error, response, body) {
  if (!error && response.statusCode == 200) {
  	var jsonData = JSON.parse(body);
  	return jsonDataresponse.results;
  }
})
}

function extractImageURI(main){
	$ = cheerio.load(main);
	var imgurl = $('figure img').attr('src');
	imgurl = imgurl.substring(0, imgurl.length);
	return imgurl;
}

var server = restify.createServer();
server.get('/hello/:name', respond);
server.head('/hello/:name', respond);

server.get('/getallnews', getAllNews);


server.listen(8080, function() {
  console.log('%s listening at %s', server.name, server.url);
});



