var restify = require('restify');
var cheerio = require('cheerio');
var mongoose = require('mongoose');
var db = mongoose.connection;
var request = require('request');

db.on('error', console.error);
db.once('open', function() {
	// Create your schemas and models here.
	var ArticleSchema = new mongoose.Schema({
		id: String,
		picurl: String,
		headline: String,
		trailtext: String,
		url: String
	});

	Article = mongoose.model('Article', ArticleSchema);

});

mongoose.connect('mongodb://localhost/test');


function saveNewsToDb(obj){
	var article = new Article({
		id: obj.id,
		picurl: extractImageURI(obj.fields.main),
		headline: obj.fields.headline,
		trailtext: obj.fields.trailText,
		url: obj.webUrl
	});

	article.save(function(err, article) {
		if (err) return console.error(err);
		console.dir(article);
	});
}

function getAllNews(req,res){
	var data = request('http://content.guardianapis.com/search?api-key=t3myqd7scnfu4t5w8zp7jx4v&show-fields=headline,trailText,main&page-size=10', function (error, response, body) {
		if (!error && response.statusCode == 200) {
			var jsonData = JSON.parse(body);
			console.log(jsonData.response.results);
			for (var i = 0; i < jsonData.response.results.length; i++){
				saveNewsToDb(jsonData.response.results[i]);
			}
			//saveNewsToDb(jsonData.response.results);
			res.send(jsonData.response.results);
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
server.get('/getallnews', getAllNews);
//server.get('');



server.listen(8080, function() {
  console.log('%s listening at %s', server.name, server.url);
});



