using System.Runtime.CompilerServices;
using Castle.Core.Internal;

[assembly: InternalsVisibleTo(InternalsVisible.ToDynamicProxyGenAssembly2)]

#region Tests

namespace CodingExerciseTests
{
    using Xunit;
    using Moq;
    using CodingExerciseLogic;

    public class UnitTests
    {

        #region HasSameDigitAtLeast3TimesContiguous
        
        [Theory]
        // no contiguous
        [InlineData("1234567890", false)]
        // 2 times contiguous
        [InlineData("1123445677", false)]
        // 3 times contiguous
        [InlineData("1112334558", true)]
        [InlineData("1123334567", true)]
        [InlineData("1123345777", true)]
        [InlineData("1112223444", true)]
        // 4 times contiguous
        [InlineData("1111234567", true)]
        [InlineData("1234444567", true)]
        [InlineData("1234567777", true)]
        [InlineData("1111233334", true)]
        [InlineData("1222234444", true)]
        // 5 times contiguous
        [InlineData("1111133445", true)]
        [InlineData("1111122222", true)]
        [InlineData("1222223456", true)]
        [InlineData("1234455555", true)]
        // 6 times contiguous
        [InlineData("1111112345", true)]
        [InlineData("1222222345", true)]
        [InlineData("1234555555", true)]
        // 7 times contiguous
        [InlineData("1111111234", true)]
        [InlineData("1222222234", true)]
        [InlineData("1235555555", true)]
        // 8 times contiguous
        [InlineData("1111111123", true)]
        [InlineData("1222222224", true)]
        [InlineData("1255555555", true)]
        // 9 times contiguous
        [InlineData("1111111112", true)]
        [InlineData("1222222221", true)]
        [InlineData("1555555555", true)]
        // 10 times contiguous
        [InlineData("1111111111", true)]
        public void Has_PhoneNumber_SameDigitAtLeast3TimesContiguous_Test(string phone, bool expectedResponse)
        {
            // arrange
            var sut = new PhoneNumbersLikedUtils();

            // act
            bool liked = sut.HasSameDigitAtLeast3TimesContiguous(phone);

            // assert
            Assert.Equal(liked, expectedResponse);
        }

        #endregion
        
        #region HasSameDigitAtLeast4TimesAtAnyPosition
        
        [Theory]
        // 1 time
        [InlineData("1234567890", false)]
        // 2 times
        [InlineData("1213243546", false)]
        [InlineData("2213143544", false)]
        // 3 times
        [InlineData("4564564567", false)]
        [InlineData("8884999567", false)]
        // 4 times
        [InlineData("4564564564", true)]
        [InlineData("0012345600", true)]
        [InlineData("0199995678", true)]
        // 5 times
        [InlineData("4142434546", true)]
        [InlineData("4545454545", true)]
        [InlineData("0012304500", true)]
        [InlineData("0199999567", true)]
        // 6 times
        [InlineData("4142434544", true)]
        [InlineData("0012004500", true)]
        [InlineData("0199999956", true)]
        // 7 times
        [InlineData("4142444544", true)]
        [InlineData("0010004500", true)]
        [InlineData("0199999995", true)]
        // 8 times
        [InlineData("4442444544", true)]
        [InlineData("0010004000", true)]
        [InlineData("0999999995", true)]
        // 9 times
        [InlineData("4442444444", true)]
        [InlineData("1000000000", true)]
        [InlineData("9999999995", true)]
        // 10 times
        [InlineData("1111111111", true)]
        public void Has_PhoneNumber_SameDigitAtLeast4TimesAtAnyPosition_Test(string phone, bool expectedResponse)
        {
            // arrange
            var sut = new PhoneNumbersLikedUtils();

            // act
            bool liked = sut.HasSameDigitAtLeast4TimesAtAnyPosition(phone);

            // assert
            Assert.Equal(liked, expectedResponse);
        }

        #endregion

        #region IsPhoneNumberLiked
        
        public static IEnumerable<object[]> IsLikedPhoneNumberData => 
            new List<object[]>
            {
                // phone, hasSameDigitAtLeast4TimesAtAnyPosition, hasSameDigitAtLeast3TimesContiguous, expectedResponse
                new object[] { "1234567890", false, false, false },
                new object[] { "1112324256", false, true, true },
                new object[] { "1123242526", true, false, true },
                new object[] { "1112324252", true, true, true },
            };
        
        [Theory, MemberData(nameof(IsLikedPhoneNumberData))]
        public void PhoneNumber_IsLiked_E2ETest(
            string phone, 
            bool hasSameDigitAtLeast4TimesAtAnyPosition, 
            bool hasSameDigitAtLeast3TimesContiguous, 
            bool expectedResponse)
        {
            // arrange
            var mock = new PhoneNumbersLikedUtils();

            // act
            bool liked = mock.IsPhoneNumberLiked(phone);

            // assert
            Assert.Equal(liked, expectedResponse);
        }

        [Theory, MemberData(nameof(IsLikedPhoneNumberData))]
        public void PhoneNumber_IsLiked_UnitTest(
            string phone, 
            bool hasSameDigitAtLeast4TimesAtAnyPosition, 
            bool hasSameDigitAtLeast3TimesContiguous, 
            bool expectedResponse)
        {
            // arrange
            var sutMock = new Mock<PhoneNumbersLikedUtils>() { CallBase = true };
            sutMock.Setup(x => x.HasSameDigitAtLeast4TimesAtAnyPosition(phone))
                .Returns(hasSameDigitAtLeast4TimesAtAnyPosition);
            sutMock.Setup(x => x.HasSameDigitAtLeast3TimesContiguous(phone))
                .Returns(hasSameDigitAtLeast3TimesContiguous);

            // act
            bool liked = sutMock.Object.IsPhoneNumberLiked(phone);

            // assert
            Assert.Equal(liked, expectedResponse);
        }
        
        #endregion
        
        #region ArePhoneNumbersLiked
        
        public static IEnumerable<object[]> AreLikedPhoneNumbersData => 
            new List<object[]>
            {
                // phones, expectedResponse
                
                // 1
                new object[] { new[] { "1234567890" }, 
                    new[] { false } },
                new object[] { new[] { "1112324252" }, 
                    new[] { true } },
                // 2
                new object[] { new[] { "1234567890", "1112324252" }, 
                    new[] { false, true } },
                // 3
                new object[] { new[] { "1234567890", "1123456788", "1112324256" }, 
                    new[] { false, false, true } },
                // 4
                new object[] { new[] { "1234567890", "1123456788", "1112324256", "1123242526" }, 
                    new[] { false, false, true, true } },
                // 5
                new object[] { new[] { "1234567890", "1123456788", "1122456788", "1112324256", "1123242526" }, 
                    new[] { false, false, false, true, true } },
            };
        
        [Theory, MemberData(nameof(AreLikedPhoneNumbersData))]
        public void PhoneNumbers_AreLiked_E2ETest(
            string[] phones, 
            bool[] expectedResponse)
        {
            // arrange
            var mock = new PhoneNumbersLikedUtils();

            // act
            bool[] likedArray = mock.ArePhoneNumbersLiked(phones);

            // assert
            Assert.Equal(likedArray, expectedResponse);
        }
        
        [Theory, MemberData(nameof(AreLikedPhoneNumbersData))]
        public void PhoneNumbers_AreLiked_UnitTest(
            string[] phones, 
            bool[] expectedResponse)
        {
            // arrange
            var sutMock = new Mock<PhoneNumbersLikedUtils>() { CallBase = true };
            for (int i = 0; i < phones.Length; i++)
            {
                sutMock.Setup(x => x.IsPhoneNumberLiked(phones[i]))
                    .Returns(expectedResponse[i]);
            }

            // act
            bool[] likedArray = sutMock.Object.ArePhoneNumbersLiked(phones);

            // assert
            Assert.Equal(likedArray, expectedResponse);
        }
        
        #endregion

    }

}

#endregion

#region Logic

namespace CodingExerciseLogic
{

    public class PhoneNumbersLikedUtils
    {
        private const int MinNumTimesAtAnyPosition = 4;
        private const int MinNumTimesContiguous = 3;
        
        #region ArePhoneNumbersLiked
        
        public bool[] ArePhoneNumbersLiked(string[] phones)
        {
            return phones.Select(IsPhoneNumberLiked)
                .ToArray();
        }

        public virtual bool IsPhoneNumberLiked(string phone)
        {
            return HasSameDigitAtLeast4TimesAtAnyPosition(phone) ||
                   HasSameDigitAtLeast3TimesContiguous(phone);
        }
        
        #endregion
        
        #region HasSameDigitAtLeast3TimesContiguous

        public virtual bool HasSameDigitAtLeast3TimesContiguous(string phone)
        {
            const string digits = "0123456789";
            return digits.Any((char digit) => HasSameDigitAtLeast3TimesContiguous(phone, digit));
        }

        internal bool HasSameDigitAtLeast3TimesContiguous(string phone, char digit)
        {
            int currNumTimesContiguous = 0, maxTimesContiguous = 0;
            for (var i = 0; i < phone.Length; i++)
            {
                if (phone[i] != digit)
                    continue;

                if (i == 0)
                {
                    currNumTimesContiguous++;
                    continue;
                }

                if (phone[i] == phone[i-1])
                {
                    currNumTimesContiguous++;

                    if (currNumTimesContiguous >= MinNumTimesContiguous)
                        return true;
                }
                else
                {
                    maxTimesContiguous = Math.Max(maxTimesContiguous, currNumTimesContiguous);
                    currNumTimesContiguous = 1;
                }
            }

            return false;
        }

        #endregion

        #region HasSameDigitAtLeast4TimesAtAnyPosition

        public virtual bool HasSameDigitAtLeast4TimesAtAnyPosition(string phone)
        {
            const string digits = "0123456789";
            return digits.Any((char digit) => HasSameDigitAtLeast4TimesAtAnyPosition(phone, digit));
        }
        
        internal bool HasSameDigitAtLeast4TimesAtAnyPosition(string phone, char digit)
        {
            int numTimes = 0;
            foreach (char c in phone)
            {
                if (c == digit)
                {
                    numTimes++;

                    if (numTimes >= MinNumTimesAtAnyPosition)
                        return true;
                }
            }
            return false;
        }

        #endregion
        
    }

    class Application { 
  
        // Main Method 
        static public void Main_(string[] args)
        {
            string[] phoneNumbers = ReadPhoneNumbersFromConsole();
            
            PhoneNumbersLikedUtils utils = new();
            bool[] likedFlags = utils.ArePhoneNumbersLiked(args);

            foreach (bool b in likedFlags)
            {
                Console.WriteLine(b ? "Y" : "N");
            }
        }

        private static string[] ReadPhoneNumbersFromConsole()
        {
            int count = Convert.ToInt32(Console.ReadLine());
            string[] phoneNumbers = new string[count];
            for (int i = 0; i < count; i++)
            {
                phoneNumbers[i] = Console.ReadLine();
            }
            return phoneNumbers;
        }
    } 
}

#endregion
