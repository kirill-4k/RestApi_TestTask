using PatientDB.Models;

namespace RestApi.Service
{
	public class QueryParser
	{
		bool _isValid = false;
		bool _isDateOnly = true;
		string _dateStringIncoming;
		Prefix _currentPrefix = Prefix.undefined;
		DateTimeOffset _startDate;
		DateTimeOffset _endDate;
		public QueryParser(string parameter)
		{
			if (string.IsNullOrWhiteSpace(parameter))
				return;
			foreach (Prefix item in Enum.GetValues(typeof(Prefix)))
			{
				if (parameter.StartsWith(item.ToString(), StringComparison.InvariantCultureIgnoreCase))
				{
					_currentPrefix = item;
					_dateStringIncoming = parameter.Substring(2, parameter.Length - 2);
					break;
				}
			}
			if (!DateTimeOffset.TryParse(_dateStringIncoming, out var incomingDate))
			{
				return;
			}
			else if (_dateStringIncoming.Length > 10) //contains HH:mm etc
			{

				_isDateOnly = false;
			}
			switch (_currentPrefix)
			{
				case Prefix.undefined: break;
				case Prefix.eq:
					if (_isDateOnly)
					{
						_startDate = incomingDate.Date;
						_endDate = incomingDate.Date + TimeSpan.FromDays(1) - TimeSpan.FromMilliseconds(1);
					}
					else _startDate = _endDate = incomingDate.DateTime;
					break;
				case Prefix.ne:
					if (_isDateOnly)
					{
						_startDate = incomingDate.Date - TimeSpan.FromMilliseconds(1);
						_endDate = incomingDate.Date + TimeSpan.FromDays(1) + TimeSpan.FromMilliseconds(1);
					}
					else _startDate = _endDate = incomingDate.DateTime;
					break;
				case Prefix.gt:
				case Prefix.sa:
					_startDate = incomingDate.DateTime + TimeSpan.FromMilliseconds(1);
					_endDate = DateTime.MaxValue; break;
				case Prefix.lt:
					_startDate = DateTime.MinValue;
					_endDate = incomingDate.DateTime - TimeSpan.FromMilliseconds(1); break;
				case Prefix.ge:
					_startDate = incomingDate.DateTime;
					_endDate = DateTime.MaxValue; break;
				case Prefix.le:
					_startDate = DateTime.MinValue;
					_endDate = incomingDate.DateTime; break;
				case Prefix.ap:
					_startDate = incomingDate.DateTime - TimeSpan.FromDays(7);
					_endDate = incomingDate.DateTime + TimeSpan.FromDays(7);
					break;

			}

			_isValid = true;
		}

		public bool IsValidQuery { get { return _isValid; } }

		
		public Func<Person, bool> GetPredicate()
		{
			if (_isValid == false) throw new InvalidDataException("Can not apply filter parameter");
			Func<Person, bool> predicate;

			if (_currentPrefix == Prefix.ne)
			{
				predicate = (x) =>
				{
					return
					(x.BirthDate >= DateTime.MinValue && x.BirthDate <= _startDate.DateTime)
					|| (x.BirthDate >= _endDate.DateTime && x.BirthDate <= DateTime.MaxValue);
				};
			}
			else predicate = (x) => { return (x.BirthDate >= _startDate.DateTime && x.BirthDate <= _endDate.DateTime); };

			return predicate;
		}

		enum Prefix
		{
			undefined,
			eq,
			ne,
			gt,
			lt,
			ge,
			le,
			sa,
			eb,
			ap
		}
	}
}
