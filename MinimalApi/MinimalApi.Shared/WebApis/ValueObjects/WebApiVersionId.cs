﻿using MinimalApi.Dom.Common.Models;

namespace MinimalApi.Dom.WebApis.ValueObjects;

public sealed class WebApiVersionId : AggregateRootId<int>
{
    public override int Value { get; protected set; }

    private WebApiVersionId() { }
    private WebApiVersionId(int value)
    {
        Value = value;
    }

    public static WebApiVersionId Create(int value) => new(value);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}