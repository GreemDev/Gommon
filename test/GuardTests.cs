using System;
using Xunit;

namespace Gommon.Tests;

public class GuardTests
{
    public static void Test_Matches()
    {
        Assert.Throws<ValueException>(() => Guard.Matches("hello 1234", @"\w+\d+"));
        Guard.Matches("1234hello", @"\d+\w+");
    }

    public static void Test_Require()
    {
        Assert.Throws<NullReferenceException>(() => Guard.RequireObject(null));
        Assert.Throws<ValueException>(() => Guard.RequireObject(null, ""));
    }

    public static void Test_Snowflake()
    {
        Assert.Throws<ValueException>(() => Guard.ValidSnowflake("1685484419 39509248"));
        Guard.ValidSnowflake("168548441939509248");
    }
}