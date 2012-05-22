using System;
using System.Collections;

namespace NBi.Core.Query
{
    public class CollectionEngine
    {
        public delegate void ExecuteValidation(ICollection coll);
        private ExecuteValidation Validations;
        private Result _validationResult;
        
        public int? Exactly {get; set;}
        public int? MoreThan { get; set; }
        public int? LessThan { get; set; }

        public Result Validate(ICollection coll)
        {
            _validationResult = Result.Success();
            if (CheckDefinition())
                Validations.Invoke(coll);
            else
                throw new Exception();

            return _validationResult;
        }
  
        private bool CheckDefinition()
        {
            if (Exactly.HasValue)
                Validations += ValidateExactly;
            if (MoreThan.HasValue)
                Validations += ValidateMoreThan;
            if (LessThan.HasValue)
                Validations += ValidateLessThan;
            
            return (Validations!=null);
        }

        private void ValidateExactly(ICollection coll)
        {
            if (coll.Count == Exactly.Value)
                _validationResult.Append(Result.Success());
            else
                _validationResult.Append(Result.Failed(string.Format("Expected exactly {0} but was {1}", Exactly.Value, coll.Count)));
        }

        private void ValidateMoreThan(ICollection coll)
        {
            if (coll.Count > MoreThan.Value)
                _validationResult.Append(Result.Success());
            else
                _validationResult.Append(Result.Failed(string.Format("Expected more than {0} but was {1}", MoreThan.Value, coll.Count)));
        }

        private void ValidateLessThan(ICollection coll)
        {
            if (coll.Count < LessThan.Value)
                _validationResult.Append(Result.Success());
            else
                _validationResult.Append(Result.Failed(string.Format("Expected less than {0} but was {1}", LessThan.Value, coll.Count)));
        }
    }
}
