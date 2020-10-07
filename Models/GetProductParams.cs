namespace CallawayPreOwnedService.Models
{
    public abstract class GetProductParams 
    {
        public string ProductID {  get; protected set; }
        public string CGID { get; protected set; }

        public string GenderHand { get; protected set; }
    }

    public class MensRightApexIronsParams : GetProductParams
    {
        public MensRightApexIronsParams()
        {
            this.ProductID = "irons-2014-apex";
            this.CGID = "single-irons";
            this.GenderHand = GenderHands.MensRight;
        }
    }
}