namespace Server.Strategy.ShipPlacement
{
    public class ShipHelper
    {
        public bool result;
        public int size;
        public int posX;
        public int posY;
        public bool vertical;

        public ShipHelper()
        {
        }

        public ShipHelper(bool result, int size, int posX, int posY, bool vertical)
        {
            this.result = result;
            this.size = size;
            this.posX = posX;
            this.posY = posY;
            this.vertical = vertical;
        }
    }
}
