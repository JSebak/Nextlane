namespace B.Data
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        #region Navigation Properties
        public ICollection<Pedido> Pedidos { get; set; }
        #endregion
    }
}
