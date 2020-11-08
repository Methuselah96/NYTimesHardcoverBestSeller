using FluentAssertions;
using NUnit.Framework;
using NYTimesHardcoverBestSellers;

namespace NYTimesHardcoverBestSellersTests
{
    public class Tests
    {
        [Test]
        public void InitialTest()
        {
            var text = @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This October 12, 1931 Last Weeks 
Week Fiction Week On List 
    
1 THE TEN COMMANDMENTS, by Warwick Deeping. (Knopf.) -- 1 
    
2 FINCHE'S FORTUNE, by Mazo de la Roche. (Little, Brown.) -- 1 
    
3 THE GOOD EARTH, by Pearl S. Buck. (John Day.) -- 1 
    
4 SHADOWS ON THE ROCK, by Willa Cather. (Knopf.) -- 1 
    
5 SCARMOUCHE THE KING MAKER, by Rafael Sabatini. (Houghton, Mifflin.) -- 1 
 
Hawes Publications  www.hawes.com ";
            var bestSellerList = TextParser.Parse(text);
            bestSellerList.Genre.Should().Be(Genre.Fiction);
            bestSellerList.Entries.Count.Should().Be(5);
            bestSellerList.Entries[0].Title.Should().Be("THE TEN COMMANDMENTS, by Warwick Deeping.");
            bestSellerList.Entries[1].Title.Should().Be("FINCHE'S FORTUNE, by Mazo de la Roche.");
            bestSellerList.Entries[2].Title.Should().Be("THE GOOD EARTH, by Pearl S. Buck.");
            bestSellerList.Entries[3].Title.Should().Be("SHADOWS ON THE ROCK, by Willa Cather.");
            bestSellerList.Entries[4].Title.Should().Be("SCARMOUCHE THE KING MAKER, by Rafael Sabatini.");
            bestSellerList.Entries[0].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[1].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[2].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[3].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[4].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[0].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[1].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[2].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[3].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[4].WeeksOnList.Should().Be(1);
        }

        [Test]
        public void NonFictionInitialTest()
        {
            var text = @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This October 12, 1931 Last Weeks 
Week Non-Fiction Week On List 
    
ELLEN TERRY AND BERNARD SHAW: A CORRESPONDENCE, by Ellen Terry 
1 -- 1 
and Bernard Shaw. (Putnam.) 
    
2 THE EPIC OF AMERICA, by James Truslow Adams. (Little, Brown.) -- 1 
    
3 MAN'S OWN SHOW: CIVILIZATION, by George Dorsey. (Harper.) -- 1 
    
WASHINGTON MERRY-GO-ROUND, by Anonymous. (Drew Pearson and Robert 
4 -- 1 
Allen.) (Liveright.) 
 
Hawes Publications  www.hawes.com ";
            var bestSellerList = TextParser.Parse(text);
            bestSellerList.Genre.Should().Be(Genre.NonFiction);
            bestSellerList.Entries.Count.Should().Be(4);
            bestSellerList.Entries[0].Title.Should().Be("ELLEN TERRY AND BERNARD SHAW: A CORRESPONDENCE, by Ellen Terry and Bernard Shaw.");
            bestSellerList.Entries[1].Title.Should().Be("THE EPIC OF AMERICA, by James Truslow Adams.");
            bestSellerList.Entries[2].Title.Should().Be("MAN'S OWN SHOW: CIVILIZATION, by George Dorsey.");
            bestSellerList.Entries[3].Title.Should().Be("WASHINGTON MERRY-GO-ROUND, by Anonymous.");
            bestSellerList.Entries[0].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[1].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[2].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[3].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[0].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[1].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[2].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[3].WeeksOnList.Should().Be(1);
        }

        [Test]
        public void AnotherParserTest()
        {
            var text = @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This November 16, 1931 Last Weeks 
Week Non-Fiction Week On List 
    
1 ELLEN TERRY AND BERNARD SHAW: A CORRESPONDENCE, by Ellen Terry -- 2 
and Bernard Shaw. (Putnam.) 
    
2 EPIC OF AMERICA, by James Truslow Adams. (Little, Brown.) -- 2 
    
3 WASHINGTON MERRY-GO-ROUND, by Anonymous. (Drew Pearson and Robert -- 2 
Allen.) (Liveright.) 
    
4 NEWTON D. BAKER, by Frederic Palmer. (Dodd, Mead.) -- 1 
    
5 MOURNING BECOMES ELECTRA, by Eugene O’Neill. (Liveright.) -- 1 
 
Hawes Publications  www.hawes.com ";
            var bestSellerList = TextParser.Parse(text);
            bestSellerList.Genre.Should().Be(Genre.NonFiction);
            bestSellerList.Entries.Count.Should().Be(5);
            bestSellerList.Entries[0].Title.Should().Be("ELLEN TERRY AND BERNARD SHAW: A CORRESPONDENCE, by Ellen Terry and Bernard Shaw.");
            bestSellerList.Entries[1].Title.Should().Be("EPIC OF AMERICA, by James Truslow Adams.");
            bestSellerList.Entries[2].Title.Should().Be("WASHINGTON MERRY-GO-ROUND, by Anonymous.");
            bestSellerList.Entries[3].Title.Should().Be("NEWTON D. BAKER, by Frederic Palmer.");
            bestSellerList.Entries[4].Title.Should().Be("MOURNING BECOMES ELECTRA, by Eugene O’Neill.");
            bestSellerList.Entries[0].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[1].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[2].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[3].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[4].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[0].WeeksOnList.Should().Be(2);
            bestSellerList.Entries[1].WeeksOnList.Should().Be(2);
            bestSellerList.Entries[2].WeeksOnList.Should().Be(2);
            bestSellerList.Entries[3].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[4].WeeksOnList.Should().Be(1);
        }

        [Test]
        public void AnotherParserTest2()
        {
            var text = @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This April 29, 1940 Last Weeks 
Week Fiction Week On List 
    
1 HOW GREEN WAS MY VALLEY, by Richard Llewellyn. (Macmillan.) 1 11 
    
2 KING'S ROW, by Henry Bellamann. (Simon & Schuster.) 5 2 
    
3 MRS. SKEFFINGTON, by Elizabeth von Arnim. (Doubleday, Doran.) 2 3 
    
4 CHAD HANNA, by Walter D. Edmonds. (Little, Brown.) 3 3 
    
5 NATIVE SON, by Richard Wright. (Harper.) 4 8 
    
6 BETHEL MERRIDAY, by Sinclair Lewis. (Doubleday, Doran.) 6 5 
 
 
Hawes Publications  www.hawes.com ";
            var bestSellerList = TextParser.Parse(text);
            bestSellerList.Genre.Should().Be(Genre.Fiction);
            bestSellerList.Entries.Count.Should().Be(6);
            bestSellerList.Entries[0].Title.Should().Be("HOW GREEN WAS MY VALLEY, by Richard Llewellyn.");
            bestSellerList.Entries[1].Title.Should().Be("KING'S ROW, by Henry Bellamann.");
            bestSellerList.Entries[2].Title.Should().Be("MRS. SKEFFINGTON, by Elizabeth von Arnim.");
            bestSellerList.Entries[3].Title.Should().Be("CHAD HANNA, by Walter D. Edmonds.");
            bestSellerList.Entries[4].Title.Should().Be("NATIVE SON, by Richard Wright.");
            bestSellerList.Entries[5].Title.Should().Be("BETHEL MERRIDAY, by Sinclair Lewis.");
            bestSellerList.Entries[0].LastWeek.Should().Be(1);
            bestSellerList.Entries[1].LastWeek.Should().Be(5);
            bestSellerList.Entries[2].LastWeek.Should().Be(2);
            bestSellerList.Entries[3].LastWeek.Should().Be(3);
            bestSellerList.Entries[4].LastWeek.Should().Be(4);
            bestSellerList.Entries[5].LastWeek.Should().Be(6);
            bestSellerList.Entries[0].WeeksOnList.Should().Be(11);
            bestSellerList.Entries[1].WeeksOnList.Should().Be(2);
            bestSellerList.Entries[2].WeeksOnList.Should().Be(3);
            bestSellerList.Entries[3].WeeksOnList.Should().Be(3);
            bestSellerList.Entries[4].WeeksOnList.Should().Be(8);
            bestSellerList.Entries[5].WeeksOnList.Should().Be(5);
        }

        [Test]
        public void AnotherParserTest3()
        {
            var text = @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This November 6, 1939 Last Weeks 
Week Fiction Week On List 
    
1 ESCAPE, by Ethel Vance. (Little, Brown.) 1 6 
    
2 CHRISTMAS HOLIDAY, by W. Somerset Maugham. (Doubleday, Doran.) 3 2 
    
3 THE GRAPES OF WRATH, by John Steinbeck. (Viking.) 2 29 
    
4 THE NAZARENE, by Sholem Asch. (Putnam.) 4 2 
    
5 SEA TOWER, by Hugh Walpole. (Doubleday, Doran.) -- 1 
    
6 CHRIST IN CONCRETE, by Pietro di Donato. (Bobbs-Merrill.) 5 10 
    
7 IT TAKES ALL KINDS, by Louis Bromfield. (Harper.) 6 3 
Hawes Publications  www.hawes.com ";
            var bestSellerList = TextParser.Parse(text);
            bestSellerList.Genre.Should().Be(Genre.Fiction);
            bestSellerList.Entries.Count.Should().Be(7);
            bestSellerList.Entries[0].Title.Should().Be("ESCAPE, by Ethel Vance.");
            bestSellerList.Entries[1].Title.Should().Be("CHRISTMAS HOLIDAY, by W. Somerset Maugham.");
            bestSellerList.Entries[2].Title.Should().Be("THE GRAPES OF WRATH, by John Steinbeck.");
            bestSellerList.Entries[3].Title.Should().Be("THE NAZARENE, by Sholem Asch.");
            bestSellerList.Entries[4].Title.Should().Be("SEA TOWER, by Hugh Walpole.");
            bestSellerList.Entries[5].Title.Should().Be("CHRIST IN CONCRETE, by Pietro di Donato.");
            bestSellerList.Entries[6].Title.Should().Be("IT TAKES ALL KINDS, by Louis Bromfield.");
            bestSellerList.Entries[0].LastWeek.Should().Be(1);
            bestSellerList.Entries[1].LastWeek.Should().Be(3);
            bestSellerList.Entries[2].LastWeek.Should().Be(2);
            bestSellerList.Entries[3].LastWeek.Should().Be(4);
            bestSellerList.Entries[4].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[5].LastWeek.Should().Be(5);
            bestSellerList.Entries[6].LastWeek.Should().Be(6);
            bestSellerList.Entries[0].WeeksOnList.Should().Be(6);
            bestSellerList.Entries[1].WeeksOnList.Should().Be(2);
            bestSellerList.Entries[2].WeeksOnList.Should().Be(29);
            bestSellerList.Entries[3].WeeksOnList.Should().Be(2);
            bestSellerList.Entries[4].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[5].WeeksOnList.Should().Be(10);
            bestSellerList.Entries[6].WeeksOnList.Should().Be(3);
        }

        [Test]
        public void AnotherParserTest4()
        {
            var text = @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This November 3, 1941 Last Weeks 
Week Non-Fiction Week On List 
    
1 BERLIN DIARY, by William L. Shirer. (Knopf.) 1 19 
    
2 THAT DAY ALONE, by Pierre van Paassen. (Dial.) 3 2 
    
A TREASURY OF GILBERT AND SULLIVAN, edited by Deems Taylor. (Simon & 
3 -- 1 
Schuster.) 
    
4 BIG FAMILY, by Bellamy Partridge. (Whittlesey.) 6 4 
    
5 READING I'VE LIKED, by Clifton Fadiman. (Simon & Schuster.) 5 5 
    
6 SECRET HISTORY OF THE AMERICAN REVOLUTION, by Carl Van Doren. 
2 4 
(Viking.) 
 
Hawes Publications  www.hawes.com ";
            var bestSellerList = TextParser.Parse(text);
            bestSellerList.Genre.Should().Be(Genre.NonFiction);
            bestSellerList.Entries.Count.Should().Be(6);
            bestSellerList.Entries[0].Title.Should().Be("BERLIN DIARY, by William L. Shirer.");
            bestSellerList.Entries[1].Title.Should().Be("THAT DAY ALONE, by Pierre van Paassen.");
            bestSellerList.Entries[2].Title.Should().Be("A TREASURY OF GILBERT AND SULLIVAN, edited by Deems Taylor.");
            bestSellerList.Entries[3].Title.Should().Be("BIG FAMILY, by Bellamy Partridge.");
            bestSellerList.Entries[4].Title.Should().Be("READING I'VE LIKED, by Clifton Fadiman.");
            bestSellerList.Entries[5].Title.Should().Be("SECRET HISTORY OF THE AMERICAN REVOLUTION, by Carl Van Doren.");
            bestSellerList.Entries[0].LastWeek.Should().Be(1);
            bestSellerList.Entries[1].LastWeek.Should().Be(3);
            bestSellerList.Entries[2].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[3].LastWeek.Should().Be(6);
            bestSellerList.Entries[4].LastWeek.Should().Be(5);
            bestSellerList.Entries[5].LastWeek.Should().Be(2);
            bestSellerList.Entries[0].WeeksOnList.Should().Be(19);
            bestSellerList.Entries[1].WeeksOnList.Should().Be(2);
            bestSellerList.Entries[2].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[3].WeeksOnList.Should().Be(4);
            bestSellerList.Entries[4].WeeksOnList.Should().Be(5);
            bestSellerList.Entries[5].WeeksOnList.Should().Be(4);
        }

        [Test]
        public void AnotherParserTest5()
        {
            var text = @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This September 29, 1946 Last Weeks 
Week Fiction Week On List 
    
1 THE HUCKSTERS, by Frederic Wakeman. (Rinehart. & Company.) 1 16 
    
2 THIS SIDE OF INNOCENCE, by Taylor Caldwell. (Scribner.) 2 23 
    
3 BRITANNIA MEWS, by Margery Sharp. (Little, Brown.) 3 11 
    
4 THE SALEM FRIGATE, by John Jennings. (Doubleday.) 4 5 
    
5 RHUBARB, by H. Allen Smith. (Doubleday.) 5 6 
    
6 THE FOXES OF HARROW, by Frank Yerby. (Dial Press.) 9 32 
    
7 ANIMAL FARM, by George Orwell. (Harcourt, Brace.) (moved from nonfiction prior 
8NF 3 
week) 
    
8 ALL THE KING'S MEN, by Robert Penn Warren. (Harcourt, Brace.) 6 3 
    
9 WE HAPPY FEW, by Helen Howe. (Simon & Schuster.) 8 10 
    
10 SPOONHANDLE, by Ruth Moore. (Morrow.) 7 12 
    
11 BELL TIMSON, by Marguerite Steen. (Doubleday.) 11 6 
    
12 MISTER ROBERTS, by Thomas Heggen. (Houghton Mifflin.) 15 3 
    
13 THE AMERICAN, by Howard Fast. (Duell, Sloan & Pearce.) 10 8 
    
14 THE OLD COUNTRY, by Sholom Aleichem. (Crown Publishers, Inc.) -- 8 
    
15 THE SNAKE PIT, by Mary Jane Ward. (Random House.) 12 23 
    
16 THE DARK WOOD, by Christine Weston. (Charles Scribner.'s Sons.) -- 1 
 
Hawes Publications  www.hawes.com ";
            var bestSellerList = TextParser.Parse(text);
            bestSellerList.Genre.Should().Be(Genre.Fiction);
            bestSellerList.Entries.Count.Should().Be(16);
            bestSellerList.Entries[0].Title.Should().Be("THE HUCKSTERS, by Frederic Wakeman.");
            bestSellerList.Entries[1].Title.Should().Be("THIS SIDE OF INNOCENCE, by Taylor Caldwell.");
            bestSellerList.Entries[2].Title.Should().Be("BRITANNIA MEWS, by Margery Sharp.");
            bestSellerList.Entries[3].Title.Should().Be("THE SALEM FRIGATE, by John Jennings.");
            bestSellerList.Entries[4].Title.Should().Be("RHUBARB, by H. Allen Smith.");
            bestSellerList.Entries[5].Title.Should().Be("THE FOXES OF HARROW, by Frank Yerby.");
            bestSellerList.Entries[6].Title.Should().Be("ANIMAL FARM, by George Orwell.");
            bestSellerList.Entries[7].Title.Should().Be("ALL THE KING'S MEN, by Robert Penn Warren.");
            bestSellerList.Entries[8].Title.Should().Be("WE HAPPY FEW, by Helen Howe.");
            bestSellerList.Entries[9].Title.Should().Be("SPOONHANDLE, by Ruth Moore.");
            bestSellerList.Entries[10].Title.Should().Be("BELL TIMSON, by Marguerite Steen.");
            bestSellerList.Entries[11].Title.Should().Be("MISTER ROBERTS, by Thomas Heggen.");
            bestSellerList.Entries[12].Title.Should().Be("THE AMERICAN, by Howard Fast.");
            bestSellerList.Entries[13].Title.Should().Be("THE OLD COUNTRY, by Sholom Aleichem.");
            bestSellerList.Entries[14].Title.Should().Be("THE SNAKE PIT, by Mary Jane Ward.");
            bestSellerList.Entries[15].Title.Should().Be("THE DARK WOOD, by Christine Weston.");
            bestSellerList.Entries[0].LastWeek.Should().Be(1);
            bestSellerList.Entries[1].LastWeek.Should().Be(2);
            bestSellerList.Entries[2].LastWeek.Should().Be(3);
            bestSellerList.Entries[3].LastWeek.Should().Be(4);
            bestSellerList.Entries[4].LastWeek.Should().Be(5);
            bestSellerList.Entries[5].LastWeek.Should().Be(9);
            bestSellerList.Entries[6].LastWeek.Should().Be(8);
            bestSellerList.Entries[7].LastWeek.Should().Be(6);
            bestSellerList.Entries[8].LastWeek.Should().Be(8);
            bestSellerList.Entries[9].LastWeek.Should().Be(7);
            bestSellerList.Entries[10].LastWeek.Should().Be(11);
            bestSellerList.Entries[11].LastWeek.Should().Be(15);
            bestSellerList.Entries[12].LastWeek.Should().Be(10);
            bestSellerList.Entries[13].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[14].LastWeek.Should().Be(12);
            bestSellerList.Entries[15].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[0].WeeksOnList.Should().Be(16);
            bestSellerList.Entries[1].WeeksOnList.Should().Be(23);
            bestSellerList.Entries[2].WeeksOnList.Should().Be(11);
            bestSellerList.Entries[3].WeeksOnList.Should().Be(5);
            bestSellerList.Entries[4].WeeksOnList.Should().Be(6);
            bestSellerList.Entries[5].WeeksOnList.Should().Be(32);
            bestSellerList.Entries[6].WeeksOnList.Should().Be(3);
            bestSellerList.Entries[7].WeeksOnList.Should().Be(3);
            bestSellerList.Entries[8].WeeksOnList.Should().Be(10);
            bestSellerList.Entries[9].WeeksOnList.Should().Be(12);
            bestSellerList.Entries[10].WeeksOnList.Should().Be(6);
            bestSellerList.Entries[11].WeeksOnList.Should().Be(3);
            bestSellerList.Entries[12].WeeksOnList.Should().Be(8);
            bestSellerList.Entries[13].WeeksOnList.Should().Be(8);
            bestSellerList.Entries[14].WeeksOnList.Should().Be(23);
            bestSellerList.Entries[15].WeeksOnList.Should().Be(1);
        }

        [Test]
        public void AnotherParserTest6()
        {
            var text = @" 
Uif!Ofx!Zpsl!Ujnft!Cftu!Tfmmfs!Mjtu!
This June 22, 1947 Last Weeks 
Week Fiction Week On List 
    
1 GENTLEMAN'S AGREEMENT, by Laura Z. Hobson. (Simon & Schuster.) 1 15 
    
2 KINGSBLOOD ROYAL, by Sinclair Lewis. (Random House.) 7 2 
    
3 THE VIXENS, by Frank Yerby. (Dial Press.) 2 7 
    
4 THERE WAS A TIME, by Taylor Caldwell. (Scribner.) 4 5 
    
5 THE MIRACLE OF THE BELLS, by Russell Janney. (Prentice Hall.) 3 38 
    
6 THE BIG SKY, by A.B. Guthrie Jr. (Pocket Books.) 5 5 
    
7 KNOCK ON ANY DOOR, by Willard Motley. (Appleton-Century.) 11 4 
    
8 MRS. MIKE, by Benedict Freedman and Nancy Freedman. (Coward-McCann.) 8 14 
    
9 LYDIA BAILEY, by Kenneth Roberts. (Doubleday.) 6 23 
    
10 PRESIDENTIAL MISSION, by Upton Sinclair. (Viking.) 9 3 
    
11 VERMILLION, by Idwal Jones.(Prentice-Hall.) -- 1 
    
12 THE WAYWARD BUS, by John Steinbeck. (Viking.) 10 16 
    
13 THE CHEQUER BOARD, by Nevil Shute. (Morrow.) 12 10 
    
14 THE SCARLET PATCH, by Bruce Lancaster. (Little, Brown & Company.) 13 4 
    
15 PEACE BREAKS OUT, by Angela Thirkell. (Knopf.) -- 1 
    
16 THE TIN FLUTE, by Gabrielle Roy. (Reynal & Hitchcock.) 16 4 
 
Hawes Publications  www.hawes.com ";
            var bestSellerList = TextParser.Parse(text);
            bestSellerList.Genre.Should().Be(Genre.Fiction);
            bestSellerList.Entries.Count.Should().Be(16);
            bestSellerList.Entries[0].Title.Should().Be("GENTLEMAN'S AGREEMENT, by Laura Z. Hobson.");
            bestSellerList.Entries[1].Title.Should().Be("KINGSBLOOD ROYAL, by Sinclair Lewis.");
            bestSellerList.Entries[2].Title.Should().Be("THE VIXENS, by Frank Yerby.");
            bestSellerList.Entries[3].Title.Should().Be("THERE WAS A TIME, by Taylor Caldwell.");
            bestSellerList.Entries[4].Title.Should().Be("THE MIRACLE OF THE BELLS, by Russell Janney.");
            bestSellerList.Entries[5].Title.Should().Be("THE BIG SKY, by A.B. Guthrie Jr.");
            bestSellerList.Entries[6].Title.Should().Be("KNOCK ON ANY DOOR, by Willard Motley.");
            bestSellerList.Entries[7].Title.Should().Be("MRS. MIKE, by Benedict Freedman and Nancy Freedman.");
            bestSellerList.Entries[8].Title.Should().Be("LYDIA BAILEY, by Kenneth Roberts.");
            bestSellerList.Entries[9].Title.Should().Be("PRESIDENTIAL MISSION, by Upton Sinclair.");
            bestSellerList.Entries[10].Title.Should().Be("VERMILLION, by Idwal Jones.");
            bestSellerList.Entries[11].Title.Should().Be("THE WAYWARD BUS, by John Steinbeck.");
            bestSellerList.Entries[12].Title.Should().Be("THE CHEQUER BOARD, by Nevil Shute.");
            bestSellerList.Entries[13].Title.Should().Be("THE SCARLET PATCH, by Bruce Lancaster.");
            bestSellerList.Entries[14].Title.Should().Be("PEACE BREAKS OUT, by Angela Thirkell.");
            bestSellerList.Entries[15].Title.Should().Be("THE TIN FLUTE, by Gabrielle Roy.");
            bestSellerList.Entries[0].LastWeek.Should().Be(1);
            bestSellerList.Entries[1].LastWeek.Should().Be(7);
            bestSellerList.Entries[2].LastWeek.Should().Be(2);
            bestSellerList.Entries[3].LastWeek.Should().Be(4);
            bestSellerList.Entries[4].LastWeek.Should().Be(3);
            bestSellerList.Entries[5].LastWeek.Should().Be(5);
            bestSellerList.Entries[6].LastWeek.Should().Be(11);
            bestSellerList.Entries[7].LastWeek.Should().Be(8);
            bestSellerList.Entries[8].LastWeek.Should().Be(6);
            bestSellerList.Entries[9].LastWeek.Should().Be(9);
            bestSellerList.Entries[10].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[11].LastWeek.Should().Be(10);
            bestSellerList.Entries[12].LastWeek.Should().Be(12);
            bestSellerList.Entries[13].LastWeek.Should().Be(13);
            bestSellerList.Entries[14].LastWeek.Should().NotHaveValue();
            bestSellerList.Entries[15].LastWeek.Should().Be(16);
            bestSellerList.Entries[0].WeeksOnList.Should().Be(15);
            bestSellerList.Entries[1].WeeksOnList.Should().Be(2);
            bestSellerList.Entries[2].WeeksOnList.Should().Be(7);
            bestSellerList.Entries[3].WeeksOnList.Should().Be(5);
            bestSellerList.Entries[4].WeeksOnList.Should().Be(38);
            bestSellerList.Entries[5].WeeksOnList.Should().Be(5);
            bestSellerList.Entries[6].WeeksOnList.Should().Be(4);
            bestSellerList.Entries[7].WeeksOnList.Should().Be(14);
            bestSellerList.Entries[8].WeeksOnList.Should().Be(23);
            bestSellerList.Entries[9].WeeksOnList.Should().Be(3);
            bestSellerList.Entries[10].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[11].WeeksOnList.Should().Be(16);
            bestSellerList.Entries[12].WeeksOnList.Should().Be(10);
            bestSellerList.Entries[13].WeeksOnList.Should().Be(4);
            bestSellerList.Entries[14].WeeksOnList.Should().Be(1);
            bestSellerList.Entries[15].WeeksOnList.Should().Be(4);
        }

        [Test]
        public void SplitIntoPartsTest1()
        {
            const string line = "1 THE TEN COMMANDMENTS, by Warwick Deeping. (Knopf.) -- 1 ";
            var (data, title) = TextParser.SplitIntoParts(line);
            data.Should().Be("1 -- 1 ");
            title.Should().Be("THE TEN COMMANDMENTS, by Warwick Deeping. (Knopf.) ");
        }
    }
}
