using System.Collections.Generic;

namespace Clean
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }

        public List<string> Errors { get; protected set; }

        protected Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
            Errors = new List<string>();
        }

        public static Result Success() => new Result(true);

        public static Result Failure() => new Result(false);


        public static Result Failure(string error)
        {
            var result = new Result(false);
            result.AddError(error);
            return result;
        }

        public static Result Failure(params string[] errors)
        {
            var result = new Result(false);
            result.AddError(errors);
            return result;
        }

        public void AddError(params string[] errors)
        {
            foreach (var item in errors)
            {
                AddError(item);
            }
        }

        public void AddError(string error)
        {
            if (IsSuccess)
                IsSuccess = false;

            Errors.Add(error);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; protected set; }

        protected Result(bool isSuccess) : base(isSuccess)
        {
        }

        public static new Result<T> Success() => new Result<T>(true);
        public static Result<T> Success(T value) => new Result<T>(true) { Value = value };

        public static new Result<T> Failure() => new Result<T>(false);

        public static new Result<T> Failure(string error)
        {
            var result = new Result<T>(false);
            result.AddError(error);
            return result;
        }

        public static new Result<T> Failure(params string[] errors)
        {
            var result = new Result<T>(false);
            result.AddError(errors);
            return result;
        }
    }
}
