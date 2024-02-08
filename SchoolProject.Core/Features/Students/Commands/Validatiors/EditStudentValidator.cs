using FluentValidation;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commands.Validatiors
{
    public class EditStudentValidator : AbstractValidator<EditStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;
        #endregion

        #region Constructors
        public EditStudentValidator(IStudentService studentService)
        {
            _studentService = studentService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name Must Not be Empty")
                                .NotNull().WithMessage("Name Must Not be Null")
                                .MaximumLength(10).WithMessage("max Length is 10");

            RuleFor(x => x.Address).NotEmpty().WithMessage("{PropertyName} Must Not be Empty")
                                   .NotNull().WithMessage("{PropertyValue} Must Not be Null")
                                   .MaximumLength(10).WithMessage("{PropertyName} Length is 10");
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Name)
                .MustAsync(async (model, Key, CancellationToken) => !await _studentService.IsNameExistExcludeSelf(Key, model.Id))
                    .WithMessage("Name Is Exist");
        }

        #endregion
    }
}
