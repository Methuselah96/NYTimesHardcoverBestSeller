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
        public void SplitIntoPartsTest1()
        {
            const string line = "1 THE TEN COMMANDMENTS, by Warwick Deeping. (Knopf.) -- 1 ";
            var (data, title) = TextParser.SplitIntoParts(line);
            data.Should().Be("1 -- 1 ");
            title.Should().Be("THE TEN COMMANDMENTS, by Warwick Deeping. (Knopf.) ");
        }
    }
}
