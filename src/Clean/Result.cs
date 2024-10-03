using System.Collections.Generic;

namespace Clean.Shared
{
    public abstract class ResultBase
    {
        public ResultBase(bool isSuccess)
        {
            IsSuccess = isSuccess;
            Errors = new List<string>();
        }

        public bool IsSuccess { get; private set; }

        public List<string> Errors { get; private set; }

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
    public partial class Result : ResultBase
    {
        internal Result(bool isSuccess) : base(isSuccess)
        {
        }
    }

    public class Result<T> : ResultBase
    {
        public T Value { get; private set; }

        internal Result(bool isSuccess, T value = default) : base(isSuccess)
        {
            Value = value;
        }

        public void SetValue(T value)
        {
            Value = value;
        }

        public static implicit operator Result<T>(Result result)
        {
            var newResult = new Result<T>(result.IsSuccess);
            newResult.AddError(result.Errors.ToArray());
            return newResult;
        }

        public static implicit operator Result(Result<T> result)
        {
            var newResult = new Result(result.IsSuccess);
            newResult.AddError(result.Errors.ToArray());
            return newResult;
        }
    }

    public partial class Result
    {
        public static Result Ok()
        {
            return new Result(true);
        }

        public static Result Failed()
        {
            return new Result(false);
        }

        public static Result Failed(string error)
        {
            var result = new Result(false);
            result.AddError(error);
            return result;
        }

        public static Result<T> Ok<T>()
        {
            return new Result<T>(true);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(true, value);
        }

        public static Result<T> Failed<T>()
        {
            return new Result<T>(false);
        }

        public static Result<T> Failed<T>(string error)
        {
            var result = new Result<T>(false);
            result.AddError(error);
            return result;
        }

        public static Result<T> Failed<T>(T value)
        {
            return new Result<T>(false, value);
        }

        public static Result<T> Failed<T>(string error, T value)
        {
            var result = new Result<T>(false, value);
            result.AddError(error);
            return result;
        }
    }
}
