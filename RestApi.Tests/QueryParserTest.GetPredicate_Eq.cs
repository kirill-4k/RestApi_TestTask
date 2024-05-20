using PatientDB.Models;
using RestApi.Service;
using Xunit;

namespace RestApi.Tests
{
	public partial class QueryParserTest
	{
		[Theory]
		[InlineData("eq2013-01-14", "2013-01-14T00:00", true)]
		[InlineData("eq2013-01-14", "2013-01-14T10:00", true)]
		[InlineData("eq2013-01-14", "2013-01-14T10:00:22", true)]
		[InlineData("eq2013-01-13", "2013-01-14T10:00:22", false)]
		[InlineData("eq2013-01-15", "2013-01-14T10:00:22", false)]
		
		[InlineData("eq2013-01-14T00:00:01", "2013-01-14T00:00:01", true)]
		[InlineData("eq2013-01-14T00:00:01", "2013-01-14T00:00:02", false)]
		[InlineData("eq2013-01-14T00:00:01", "2013-01-14T00:00:00", false)]
		public void QueryBuilder_GetPredicate_Eq(string parameter, string birthDate, bool expectedResult)
		{
			var qb = new QueryParser(parameter);

			var result = qb.GetPredicate().Invoke(new Person() { BirthDate = DateTime.Parse(birthDate) });
			
			Assert.Equal(expectedResult, result);
		}
	}
	
}