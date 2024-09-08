namespace B.Data
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Detalle { get; set; }
        public int ClienteId { get; set; }

        #region Navigation Properties
        public Cliente Cliente { get; set; }
        #endregion
    }
}
