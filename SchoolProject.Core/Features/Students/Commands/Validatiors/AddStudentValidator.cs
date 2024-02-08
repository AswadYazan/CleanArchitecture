
using FluentValidation;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Core.Features.Students.Commands.Validatiors
{
    public class AddStudentValidator : AbstractValidator<AddStudentCommand>
    {
        #region Fields
        //custom validation هي مشان 
        private readonly IStudentService _studentService;
        #endregion

        #region Constructors
        public AddStudentValidator(IStudentService studentService)
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
            //False مرجع  _studentService.IsNameExist هي! يعني ال ميثود  !await 
            RuleFor(x => x.Name)
               .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameExist(Key))
               .WithMessage("Name Is Exist");
           

            /*RuleFor(x => x.DepartmementId)
           .MustAsync(async (Key, CancellationToken) => await _departmentService.IsDepartmentIdExist(Key))
           .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);*/


        }

        #endregion

    }
}
