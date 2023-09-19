namespace BlazorClient.Shared;

public static class CustomValidations
{
    public static void NotEmptyDate(ValidatorEventArgs e)
    {
        if (e.Value == null)
        {
            e.Status = ValidationStatus.Error;
            return;
        }

        if (((DateTime)e.Value) == default)
        {
            e.Status = ValidationStatus.Error;
            return;
        }

        e.Status = ValidationStatus.Success;
    }
    
    public static void NotEmptyDecimal(ValidatorEventArgs e)
    {
        if (e.Value == null)
        {
            e.Status = ValidationStatus.Error;
            return;
        }

        e.Status = ValidationStatus.Success;
    }

}