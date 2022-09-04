namespace Assessment.Logic.Dtos.ValidationDtos
{
    public class ValidationResultDto
    {
        public ValidationResultDto()
        {
            Conditions  = new List<ValidationConditionDto>();
        }

        public List<ValidationConditionDto> Conditions { get; set; }
    }
}
