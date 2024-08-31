namespace Barber.UI.Entities
{
    public class DaysOfMonth
    {
        public int[] Days { get; set; }

        public DaysOfMonth(Month month)
        {
            if(month.ToString() == "Janeiro" || month.ToString() == "Março" 
                || month.ToString() == "Maio" || month.ToString() == "Julho" || month.ToString() == "Agosto" 
                || month.ToString() == "Outubro" || month.ToString() == "Dezembro")
            {
                Days = GenerateDays(31);
            }
            else if (month.ToString() == "Fevereiro")
            {
                Days = GenerateDays(28);
            }
            else
            {
                Days = GenerateDays(30);
            }
        }

        private int[] GenerateDays(int days)
        {
            int[] generatedDays = new int[days];
            for(int i = 1; i == days; i++)
            {
                generatedDays[i] = i;   
            }
            return generatedDays;
        }
    }
}
