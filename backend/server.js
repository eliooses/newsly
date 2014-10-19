var restify = require('restify');
var cheerio = require('cheerio');
var mongoose = require('mongoose');
var db = mongoose.connection;
var request = require('request');
var unirest = require('unirest');

db.on('error', console.error);
db.once('open', function() {
	// Create your schemas and models here.
	var ArticleSchema = new mongoose.Schema({
		id: String,
		picurl: String,
		headline: String,
		trailtext: String,
		url: String,
		content: String
	});

	Article = mongoose.model('Article', ArticleSchema);

	var PreferenceSchema = new mongoose.Schema({
		key: String
	});

	Preference = mongoose.model('Preference', PreferenceSchema);

});

mongoose.connect('mongodb://localhost/test');


function saveNewsToDb(obj){
	$ = cheerio.load(obj.fields.trailText);
	var trail = $(obj.fields.trailText).text();
	var article = new Article({
		id: obj.id,
		picurl: extractImageURI(obj.fields.main),
		headline: obj.fields.headline,
		trailtext: trail,
		url: obj.webUrl,
		content: obj.fields.body
	});

	article.save(function(err, article) {
		if (err) return console.error(err);
		console.dir(article);
	});
}

function getAllNews(req,res){
	var data = request('http://content.guardianapis.com/search?api-key=t3myqd7scnfu4t5w8zp7jx4v&show-fields=headline,trailText,main,body&page-size=10', function (error, response, body) {
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

/*function getPrefNews(req,res){
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
}*/


function addPrefs(req, res){
	var incomingID = req.params.id;
	Article.findOne( {_id: incomingID}, function(err, data) {
		if (err) return console.error(err);
		//res.send(data.content);
		getKeywords(data.content);
		//console.log(data.content);
		//var mypref = prefs[Math.random() * (prefs.length - 1)];
		/*
		var preference = new Preference({
			key:mypref
		});

		preference.save(function(err, preference){
			if (err) console.error(err);
			console.dir(preference);
			res.send(preference);
		});
		*/
		// Preferences.findOne add prefs[0]

	});
	//take in id
	//query for body of article from id
	//use get keywords
	// store in preferences

}


function getKeywords(content){
	unirest.post("https://joanfihu-article-analysis-v1.p.mashape.com/text")
	.header("X-Mashape-Key", "2LKLhCuMs2mshs6s3OxvtL2325czp1JNAz8jsnz6QtbmGesuEv")
	.header("Content-Type", "application/x-www-form-urlencoded")
	.field("text", content)
	.field('title', 'hi')
	.end(function (result) {	
		//console.log(result.status, result.headers, result.body);
		//res.send(result);
		console.log(result.body.keywords);
		return result.body.keywords;
	});
}


function extractImageURI(main){
	$ = cheerio.load(main);
	var imgurl = $('figure img').attr('src');
	imgurl = imgurl.substring(0, imgurl.length);
	return imgurl;
}

var server = restify.createServer();
server.get('/getallnews', getAllNews);
//server.get('/getkeywords', getKeywords);
server.get('/addPrefs/:id', addPrefs)
//server.get('');



server.listen(8080, function() {
  console.log('%s listening at %s', server.name, server.url);
});



