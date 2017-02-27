namespace YmtSystem.Domain.Shard
{
    using System;
    using System.Collections.Generic; 
    using YmtSystem.Domain.Shard.Validator;
    using YmtSystem.CrossCutting;

    /// <summary>
    /// 实体验证
    /// </summary>
    public static class EntityValidator
    {
        public static ExecuteResult<ValidatorResult> Validator<T>(this T val) where T : Entity
        {
            if (val == null) new ExecuteResult<ValidatorResult>(false, "entity 为空", new ValidatorResult().SetSuccess(false).SetMessage(new string[] { "实体为空"}));
            var validFactory = EntityValidatorFactory.CreateValidator();
            var valid = validFactory.IsValid<T>(val);
            var validMsg = validFactory.GetInvalidMessages(val);
            return new ExecuteResult<ValidatorResult>(valid, string.Join(";", validMsg), new ValidatorResult().SetSuccess(valid).SetMessage(validMsg));
        }
    }
}
