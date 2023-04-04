namespace ERP.Ticketing.HttpApi.Data.Common.Helpers;

public interface IModel<TKey>
{
    public TKey Id { get; set; }
}

public interface IModel: IModel<Guid> {}