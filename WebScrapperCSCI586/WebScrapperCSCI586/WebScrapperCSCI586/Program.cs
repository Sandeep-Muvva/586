using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebScrapperCSCI586
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScrapGameSpotWebsite();
            //ScrapMetaCriticsWebsite();
            ScrapIGNWebsite();
        }

        public static void ScrapGameSpotWebsite()
        {
            for (int pageNumber = 101; pageNumber <= 200; pageNumber++)
            {
                var html = @"https://www.gamespot.com/new-games/?sort=score&game_filter_type%5Bplatform%5D=&game_filter_type%5BminRating%5D=&game_filter_type%5BtimeFrame%5D=&game_filter_type%5BstartDate%5D=&game_filter_type%5BendDate%5D=&game_filter_type%5Btheme%5D=&game_filter_type%5Bregion%5D=1&game_filter_type%5Bletter%5D=&page=" + pageNumber;

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(html);

                var htmlNodes = htmlDoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("media media-game media--small"));

                foreach (var node in htmlNodes)
                {

                    var _gameSpotGameName = node.Descendants("h3").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("media-title"));



                    var _gameSpotScore = node.Descendants("strong").Where(d => d.Attributes.Contains("title") && d.Attributes["title"].Value.Contains("GameSpot Score"));
                    var _gameSpotScoreInner = "";
                    try
                    {                    
                        _gameSpotScoreInner = _gameSpotScore.First().InnerHtml;
                    }
                    catch
                    {
                        _gameSpotScoreInner = "0";
                    }
                    

                    var _gameSpotRecommendation = node.Descendants("span").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("score-word"));
                    var _gameSpotRecommendationInner = "";
                    try
                    {
                        _gameSpotRecommendationInner = _gameSpotRecommendation.First().InnerText;
                    }
                    catch
                    {
                        _gameSpotRecommendationInner = "NOT PROVIDED";
                    }


                    var _gameSpotComments = node.Descendants("p").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("media-deck"));


                    var _innerGameSpotComments = _gameSpotComments.First().InnerText;
                    _innerGameSpotComments = _innerGameSpotComments.Replace("&amp", "&");
                    _innerGameSpotComments = _innerGameSpotComments.Replace("&#039", "'");


                    var _userScoreGameSpot = node.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("media-well--review-user"));
                    var _userScoreGameSpotInner = "";
                    try
                    {

                        _userScoreGameSpotInner = _userScoreGameSpot.First().SelectSingleNode("strong").InnerText;
                    }
                    catch
                    {
                        _userScoreGameSpotInner = "NOT PROVIDED";
                    }

                    var _releaseDateGameSpot = node.Descendants("time").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("media-date"));
                    var _releaseDateGameSpotInner = _releaseDateGameSpot.First().ChildAttributes("datetime").First().Value;

                    var _platFormsAvailable = node.Descendants("ul").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("system-list"));
                    var _platFormsAvailableInner = _platFormsAvailable.First().ChildNodes.Descendants("span").Where(d => d.Attributes.Contains("itemprop") && d.Attributes["itemprop"].Value.Contains("device"));
                    var _platFormsAvailableString = "";

                    int count = 0;
                    foreach (var child in _platFormsAvailableInner)
                    {
                        if (count == 0)
                            _platFormsAvailableString = _platFormsAvailableString + child.InnerText;
                        else
                            _platFormsAvailableString = _platFormsAvailableString + " " + child.InnerText;

                        count++;
                    }

                    var _urlBaseGameSpot = @"https://www.gamespot.com";

                    var _gameSpotWiki = "";

                    try
                    {
                        var _gameSpotGameWikiAttribute = node.ChildNodes[0].Attributes[1].Value;
                        _gameSpotWiki = _urlBaseGameSpot + _gameSpotGameWikiAttribute;
                    }
                    catch
                    {
                        _gameSpotWiki = "NOT PROVIDED";
                    }

                    double _scoreParseResult = 0;
                    double _userScoreParseResult = 0;

                    Double.TryParse(_gameSpotScoreInner, out _scoreParseResult);
                    Double.TryParse(_userScoreGameSpotInner, out _userScoreParseResult);

                    var _innerGameSpotGameName = _gameSpotGameName.First().InnerHtml;

                    try
                    {
                        _innerGameSpotGameName = _innerGameSpotGameName.Replace("&amp", "&");
                        _innerGameSpotGameName = _innerGameSpotGameName.Replace("&#039", "'");

                        Regex rgx = new Regex("[^a-zA-Z0-9 &]");
                        _innerGameSpotGameName = rgx.Replace(_innerGameSpotGameName, "");
                        _innerGameSpotGameName = Regex.Replace(_innerGameSpotGameName, @"\t|\n|\r", "");
                        
                    }
                    catch
                    {
                        _innerGameSpotGameName = "NOT PROVIDED";
                    }


                    GameSpotGame _game = new GameSpotGame
                    {
                        Name = _innerGameSpotGameName,
                        Score = _scoreParseResult,
                        Recommendation = _gameSpotRecommendationInner,
                        Comments = _innerGameSpotComments,
                        UserScore = _userScoreParseResult,
                        Wiki = _gameSpotWiki,
                        ReleaseDate = _releaseDateGameSpotInner,
                        Platforms = _platFormsAvailableString
                    };

                    SaveData.SafeDataSQLDatabaseGameSpot(_game);

                    Console.WriteLine("GameSpot Game Name: " + _gameSpotGameName.First().InnerHtml);
                    Console.WriteLine("GameSpot Score: " + _gameSpotScoreInner);
                    Console.WriteLine("GameSpot Reccommendation: " + _gameSpotRecommendationInner);
                    Console.WriteLine("GameSpot Comments: " + _innerGameSpotComments);
                    Console.WriteLine("GameSpot User Score: " + _userScoreGameSpotInner);
                    Console.WriteLine("GameSpot Wiki: " + _gameSpotWiki);
                    Console.WriteLine("Release Date: " + _releaseDateGameSpotInner);
                    Console.WriteLine("Platforms Available: " + _platFormsAvailableString);

                    Console.WriteLine("//");
                }

            }

            Console.WriteLine("FINISHED GAMESPOT!");
            Console.Beep(3000, 1000);
            Console.ReadLine();
        }

        public static void ScrapMetaCriticsWebsite()
        {
            for (int pageNumber = 31; pageNumber <= 60; pageNumber++)
            {
                var html = @"http://www.metacritic.com/browse/games/score/metascore/all/all/filtered?view=detailed&sort=desc&page=" + pageNumber;

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(html);

                var htmlNodes = htmlDoc.DocumentNode.Descendants("ol").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("list_products list_product_summaries"));

                foreach (var node in htmlNodes.First().ChildNodes)
                {

                    //Name
                    var _metaCriticsGameName = "";
                    try
                    {
                        _metaCriticsGameName = node.Descendants("h3").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("product_title")).First().InnerText;

                        _metaCriticsGameName = _metaCriticsGameName.Replace("&amp", "&");
                        _metaCriticsGameName = _metaCriticsGameName.Replace("&#39", "'");
                        
                        Regex rgx = new Regex("[^a-zA-Z0-9 &]");
                        _metaCriticsGameName = rgx.Replace(_metaCriticsGameName, "");
                        _metaCriticsGameName = Regex.Replace(_metaCriticsGameName, @"\t|\n|\r", "");
                    }
                    catch
                    {
                        _metaCriticsGameName = "NOT PROVIDED";
                    }

                    var _metaCriticsScore = node.Descendants("a").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("basic_stat product_score"));


                    //Score
                    var _metaCriticsScoreInner = "";
                    try
                    {
                        _metaCriticsScoreInner = _metaCriticsScore.First().InnerText;
                        _metaCriticsScoreInner = Regex.Replace(_metaCriticsScoreInner, @"\t|\n|\r|,|\s", "");
                    }
                    catch
                    {
                        _metaCriticsScoreInner = "0";
                    }


                    //User Score
                    var _metaCriticsUserScore = node.Descendants("li").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("stat product_avguserscore"));
                    var _metaCriticsUserScoreInner = "";

                    try
                    {
                        _metaCriticsUserScoreInner = _metaCriticsUserScore.First().ChildNodes[3].InnerText;
                        _metaCriticsUserScoreInner = Regex.Replace(_metaCriticsUserScoreInner, @"\t|\n|\r|\s", "");
                    }
                    catch
                    {
                        _metaCriticsUserScoreInner = "NOT PROVIDED";
                    }

                    //Genre
                    var _metaCriticsGenreWords = node.Descendants("li").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("stat genre"));
                    var _metaCriticsGenreWordsInner = "";

                    try
                    {
                        _metaCriticsGenreWordsInner = _metaCriticsGenreWords.First().ChildNodes[3].InnerText;
                    }
                    catch
                    {
                        try
                        {
                            _metaCriticsGenreWords = node.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("product_basics product_image small_image"));
                            var _relativeUrlGame = _metaCriticsGenreWords.First().ChildNodes[0].Attributes["href"].Value;

                            var rootHtml = @"http://www.metacritic.com";
                            rootHtml = rootHtml + _relativeUrlGame;

                            var htmlDocSpecificGame = web.Load(@rootHtml);
                            var _htmlNodeMetaCriticsGenreWords = htmlDocSpecificGame.DocumentNode.Descendants("li").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("summary_detail product_genre"));
                            var _htmlNodeMetaCriticsGenreWordsInner = _htmlNodeMetaCriticsGenreWords.First().ChildNodes.Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("data"));
                            _metaCriticsGenreWordsInner = "";

                            int count = 0;
                            foreach (var child in _htmlNodeMetaCriticsGenreWordsInner)
                            {

                                    if (count == 0 && child.InnerText != "Genre(s):")
                                        _metaCriticsGenreWordsInner = _metaCriticsGenreWordsInner + child.InnerText;
                                    else if (child.InnerText != "Genre(s):")
                                        _metaCriticsGenreWordsInner = _metaCriticsGenreWordsInner + " " + child.InnerText;
                                count++;
                            }


                            _metaCriticsGenreWordsInner = _htmlNodeMetaCriticsGenreWords.First().InnerText;
                        }
                        catch(Exception e)
                        {
                            var z = e;
                            _metaCriticsGenreWordsInner = "NOT PROVIDED";
                        }
                    }


                    _metaCriticsGenreWordsInner = Regex.Replace(_metaCriticsGenreWordsInner, @"\t|\n|\r", "");
                    _metaCriticsGenreWordsInner = Regex.Replace(_metaCriticsGenreWordsInner, @",", " ");

                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    _metaCriticsGenreWordsInner = regex.Replace(_metaCriticsGenreWordsInner, " ");


                    _metaCriticsGenreWordsInner = _metaCriticsGenreWordsInner.Replace("Genre(s):", ""); 

                    //Publisher
                    var _metaCriticsPublisher = node.Descendants("li").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("stat publisher"));
                    var _metaCriticsPublisherInner = "";

                    try
                    {
                        _metaCriticsPublisherInner = _metaCriticsPublisher.First().ChildNodes[3].InnerText;
                        _metaCriticsPublisherInner = Regex.Replace(_metaCriticsPublisherInner, @"\t|\n|\r", "");
                        _metaCriticsPublisherInner = Regex.Replace(_metaCriticsPublisherInner, @",", " ");

                        RegexOptions optionsII = RegexOptions.None;
                        Regex regexII = new Regex("[ ]{2,}", options);
                        _metaCriticsPublisherInner = regex.Replace(_metaCriticsPublisherInner, " ");
                    }
                    catch
                    {
                        _metaCriticsPublisherInner = "NOT PROVIDED";
                    }

                    //Rating
                    var _metaCriticsRating = node.Descendants("li").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("stat maturity_rating"));
                    var _metaCriticsRatingInner = "";

                    try
                    {
                        _metaCriticsRatingInner = _metaCriticsRating.First().ChildNodes[3].InnerText;
                        _metaCriticsRatingInner = Regex.Replace(_metaCriticsRatingInner, @"\t|\n|\r|\s", "");
                    }
                    catch
                    {
                        _metaCriticsRatingInner = "NOT PROVIDED";
                    }



                    Console.WriteLine("MetaCritics Game Name: " + _metaCriticsGameName);
                    Console.WriteLine("MetaCritics Score: " + _metaCriticsScoreInner);
                    Console.WriteLine("MetaCritics User Score: " + _metaCriticsUserScoreInner);
                    Console.WriteLine("MetaCritics Genre: " + _metaCriticsGenreWordsInner);
                    Console.WriteLine("MetaCritics Rating: " + _metaCriticsRatingInner);
                    Console.WriteLine("MetaCritics Publisher: " + _metaCriticsPublisherInner);

                    Console.WriteLine("//");

                    double _scoreParseResult = 0;
                    double _userScoreParseResult = 0;

                    Double.TryParse(_metaCriticsScoreInner, out _scoreParseResult);
                    Double.TryParse(_metaCriticsUserScoreInner, out _userScoreParseResult);

                    MetaCriticsGame _game = new MetaCriticsGame
                    {
                        Name = _metaCriticsGameName,
                        Score = _scoreParseResult,
                        UserScore = _userScoreParseResult,
                        GenreKeyWords = _metaCriticsGenreWordsInner,
                        Rating = _metaCriticsRatingInner,
                        Publisher = _metaCriticsPublisherInner
                    };

                    SaveData.SafeDataSQLDatabaseMetaCritics(_game);


                }

            }

            
            Console.WriteLine("FINISHED METACRITICS!!");
            Console.Beep(3000, 1000);
            Console.ReadLine();
        }

        public static void ScrapIGNWebsite()
        {
            for (int pageNumber = 4600; pageNumber <= 4950; pageNumber += 50)
            {
                Thread.Sleep(1000);
                var html = @"http://www.ign.com/games?startIndex=" + pageNumber + "&sortBy=popularity&sortOrder=desc";

                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(html);

                var htmlNodes = htmlDoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("item-list"));

                var htmlNodesNextLevel = htmlNodes.First().ChildNodes[3].Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("clear itemList-itemShort"));

                int loop = 0;

                foreach (var node in htmlNodesNextLevel)
                {
                    Thread.Sleep(1000);
                    var _IGNGameName = node.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("item-title"));
                    var _IGNGameNameSecondLevel = _IGNGameName.First().ChildNodes[1];
                    var _IGNGameNameInner = _IGNGameNameSecondLevel.InnerText;
                    var _IGNGamePage = _IGNGameNameSecondLevel.Attributes["href"].Value;
                    var _IGNScore = node.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("grid_3"));

                    var _IGNScoreInner = "";

                    try
                    {
                        _IGNScoreInner = _IGNScore.First().InnerText;
                    }
                    catch
                    {
                        _IGNScoreInner = "NOT PROVIDED";

                    }


                    int retry = 0;
                    AGAIN:

                    Thread.Sleep(1000);

                    var rootHtml = @"http://www.ign.com";
                    rootHtml = @rootHtml + _IGNGamePage;

                    var _IGNWiki = "";
                    var htmlDocSpecificGame = new HtmlDocument();
                     htmlDocSpecificGame = web.Load(@rootHtml);
                    

                    try
                    {
                        var children = htmlDocSpecificGame.DocumentNode.Descendants("a");
                        var _htmlNodeIGNWiki = htmlDocSpecificGame.DocumentNode.Descendants("a").Where(d => d.Attributes.Contains("title") && d.Attributes["title"].Value.Equals("wiki-guide"));
                        _IGNWiki = @_htmlNodeIGNWiki.First().Attributes["href"].Value;
                    }
                    catch
                    {

                        //var _htmlNodeIGNWiki = htmlDocSpecificGame.DocumentNode.Descendants("a").Where(d => d.Attributes.Contains("title") && d.Attributes["title"].Value.Equals("wiki-guide"));
                        //_IGNWiki = @_htmlNodeIGNWiki.First().Attributes["href"].Value;

                        if (retry == 1 || retry == 2)
                        {
                            _IGNWiki = "NOT PROVIDED";
                        }
                        else
                        {
                            retry++;
                            goto AGAIN;
                        }
                    }


                    var _IGNUserScoreInner = "";
                    try
                    {
                        var _IGNUserScore = htmlDocSpecificGame.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("ratingValue"));
                        _IGNUserScoreInner = _IGNUserScore.ElementAt(1).InnerText;

                    }
                    catch(Exception E)
                    {
                        //var _IGNUserScore = htmlDocSpecificGame.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals("ratingValue"));
                        //_IGNUserScoreInner = _IGNUserScore.First().ChildNodes.ElementAt(1).InnerText;

                        if (retry == 1 || retry == 2)
                        {
                            _IGNUserScoreInner = "0";
                        }
                        else
                        {
                            retry++;
                            goto AGAIN;
                        }

                    }

                    try
                    {
                        _IGNGameNameInner = _IGNGameNameInner.Replace("&amp", "&");
                        _IGNGameNameInner = _IGNGameNameInner.Replace("&#039", "'");

                        Regex rgx = new Regex("[^a-zA-Z0-9 &]");
                        _IGNGameNameInner = rgx.Replace(_IGNGameNameInner, "");
                        _IGNGameNameInner = Regex.Replace(_IGNGameNameInner, @"\t|\n|\r", "");

                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex("[ ]{2,}", options);
                        _IGNGameNameInner = regex.Replace(_IGNGameNameInner, " ");

                    }
                    catch
                    {
                        _IGNGameNameInner = "NOT PROVIDED";
                    }

                    //Score
                    try
                    {
                        _IGNUserScoreInner = Regex.Replace(_IGNUserScoreInner, @"\t|\n|\r|,|\s", "");
                    }
                    catch
                    {
                            _IGNUserScoreInner = "0";
                    }

                    try
                    {
                        _IGNScoreInner = Regex.Replace(_IGNScoreInner, @"\t|\n|\r|,|\s", "");

                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex("[ ]{2,}", options);
                        _IGNScoreInner = regex.Replace(_IGNScoreInner, " ");
                    }
                    catch
                    {
                            _IGNScoreInner = "0";
                    }


                    double _scoreParseResult = 0;
                    double _userScoreParseResult = 0;

                    Double.TryParse(_IGNScoreInner, out _scoreParseResult);
                    Double.TryParse(_IGNUserScoreInner, out _userScoreParseResult);

                    IGNGame _game = new IGNGame
                    {
                        Name = _IGNGameNameInner,
                        Score = _scoreParseResult,
                        UserScore = _userScoreParseResult,
                        Wiki = _IGNWiki
                    };

                    SaveData.SafeDataSQLDatabaseIGN(_game);


                    Console.WriteLine("IGN Game Name: " + _IGNGameNameInner);
                    Console.WriteLine("IGN Score: " + _IGNScoreInner);
                    Console.WriteLine("IGN User Score: " + _IGNUserScoreInner);
                    Console.WriteLine("IGN Wiki: " + _IGNWiki);

                    Console.WriteLine("//");

                    loop++;
                }

            }

            Console.WriteLine("FINISHED IGN!");
            Console.Beep(3000, 1000);
            Console.ReadLine();
        }






        public static void CheckBitmainWebsite()
        {
            int iterations = 1;
            while (true)
            {
                Console.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] Iteration #" + iterations);
                Console.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] Loading info from bitmain...");

                DateTime _start = DateTime.Now;

                var html = @"https://shop.bitmain.com/main.htm?lang=en";

                HtmlWeb web = new HtmlWeb();

                HtmlDocument htmlDoc = new HtmlDocument();

                try
                {
                     htmlDoc = web.Load(html);
                }
                catch (System.Net.WebException e)
                {

                    Console.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] REQUEST TIMED OUT");
                    Console.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] Loading info from bitmain again...");
                    htmlDoc = web.Load(html);
                }


                DateTime _end = DateTime.Now;
                TimeSpan diff = (_end - _start);

                Console.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] Loaded in " + diff.TotalSeconds + " Seconds, checking stock now...");


                var htmlNodes = htmlDoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("homepage-prtList prtList-cart"));
                var htmlNodesSecondLevel = htmlNodes.First().ChildNodes[1];  //Where(d => d.Attributes.Contains("ul")); && d.Attributes["class"].Value.Contains("sold"));
                var htmlNodesThirdLevel = htmlNodesSecondLevel.ChildNodes.Where(d=> d.Name.Contains("li"));

                int count = 0;


                foreach (var node in htmlNodesThirdLevel)
                {
                    int sold = Regex.Matches(node.InnerHtml, "sold").Count;

                    string _productName = node.Descendants("h2").First().InnerText;

                    if (sold == 1)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] " + _productName + " Out of Stock");
                        count++;
                    }
                    else
                    {
                        Console.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] ** " + _productName + " **" + " IN STOCK!");
                    }
                }

                if (count != 3)
                {
                    Console.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "] DESIRED PRODUCT IN STOCK!");
                    for (int i = 0; i <= 10; i++)
                        Console.Beep(3000, 2000);
                }


                Console.WriteLine(" ");              
                Console.WriteLine("Waiting 30 seconds for next check iteration...");
                Thread.Sleep(30000);
                Console.WriteLine("--");
                Console.WriteLine("--");

                iterations++;

            }

        }
    }
}
