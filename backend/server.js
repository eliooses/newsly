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
		url: String
	});

	Article = mongoose.model('Article', ArticleSchema);

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


/*function userHasLiked(true){
	if (!null)

}*/

function getKeywords(){
	unirest.post("https://joanfihu-article-analysis-v1.p.mashape.com/text")
	.header("X-Mashape-Key", "2LKLhCuMs2mshs6s3OxvtL2325czp1JNAz8jsnz6QtbmGesuEv")
	.header("Content-Type", "application/x-www-form-urlencoded")
	.field("text", "A new report from Pew Research brings together almost 2,000 experts to comprehensively assess the effect of robots on the workplace</p><p>Experts are divided over the role of robots over the next decade, with some arguing that they will create more jobs than they displace, and others worrying that they could lead to income inequality and a breakdown in social order.</p><p>The findings come from <a href='http://www.pewinternet.org/packages/the-web-at-25/'>a report by Pew Research</a>, which surveyed almost two thousand individuals with expertise in artificial intelligence (AI), robotics and economics, to find out their predictions for the role of automation between today and 2025. The experts were almost perfectly split, with 52% predicting an optimistic path, and 48% worrying about the future.")
	.field('title', 'hi')
	.end(function (result) {	
		console.log(result.status, result.headers, result.body);
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
server.get('/getkeywords', getKeywords);
//server.get('');



server.listen(8080, function() {
  console.log('%s listening at %s', server.name, server.url);
});



