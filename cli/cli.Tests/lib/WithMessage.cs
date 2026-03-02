public class CustomType<T1, T2>
{
    public required T1 Value { get; set; }
    public required T2 Message { get; set; }
}

// Fix second type to string
public class CustomTypeWithString<T> : CustomType<T, string>
{
}
