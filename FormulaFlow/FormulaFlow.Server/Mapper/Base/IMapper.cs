namespace FormulaFlow.Server.Mapper.Base
{
    public interface IMapper<TFrom, TTo>
    {
        public TTo Map(TFrom from);
    }
}
