namespace otypes
{
    public struct textent
    {

        public double x1, y1, x2, y2;

        public textent(double p1, double p2, double p3, double p4)
        {
            x1 = p1; y1 = p2; x2 = p3; y2 = p4;
        }

    }

    public delegate void tmessage(object sender, string msg);

}
