using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastacture.Abstracts;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Service.Implementation
{
    class StudentService : IStudentService
    {
        #region Fields
        private readonly IStudentRepository _studentRepository;

        #endregion



        #region constructor
        public StudentService(IStudentRepository studentRepository)
        {
            this._studentRepository = studentRepository;

        }


        #endregion


        #region HandleFunctions
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _studentRepository.GetStudentsListAsync();


        }
        public async Task<Student> GetStudentByIDWithIncludeAsync(int id)
        {
            //بحيث التعليمة بتجيب كل الداتا وبتفلتر عندي list هون مستخدمين عملية جلب للداتا بطريقة   
            // var student = await _studentRepository.GetByIdAsync(id);


            //بحيث التعليمة بتجيب بس الداتا أو السطر المطلوب  IQueryable هون مستخدمين عملية جلب للداتا بطريقة   
            //بعدين بتجيب النتيجة
            //Logic Operations

            var student = _studentRepository.GetTableNoTracking()
                                                  .Include(x => x.Department)
                                                  .Where(x => x.StudID.Equals(id))
                                                  .FirstOrDefault();
            return student;


        }

        public async Task<string> AddAsync(Student student)
        {
            //check if user exist (thst is logic)

            var result = _studentRepository.GetTableNoTracking().Where(x => x.Name.Equals(student.Name)).FirstOrDefault();
            if (result != null) return "Exist";

            //Added Student
            await _studentRepository.AddAsync(student);
            return "Success";






        }


        public async Task<bool> IsNameExist(string name)
        {
            //Check if the name is Exist Or not
            var student = _studentRepository.GetTableNoTracking().Where(x => x.Name.Equals(name)).FirstOrDefault();
            if (student == null) return false;
            return true;
        }


        public async Task<bool> IsNameExistExcludeSelf(string name, int id)
        {
            //Check if the name is Exist Or not
            //التأكد من عدم تعديل الاسم لاسم موجود سابقا مع اي دي آخر 
            //اما تعديل غير الاسم مثل التاريخ والعنوان لقيم موجودة مع رق معرف اخر فهو مسموح
            var student = await _studentRepository.GetTableNoTracking().Where(x => x.Name.Equals(name) & !x.StudID.Equals(id)).FirstOrDefaultAsync();

            if (student == null) return false;
            return true;
        }

        public async Task<string> EditAsync(Student student)
        {
            await _studentRepository.UpdateAsync(student);
            return "Success";
        }
        public async Task<string> DeleteAsync(Student student)
        {

            var trans = _studentRepository.BeginTransaction();
            try
            {
                await _studentRepository.DeleteAsync(student);
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "Falied";
            }
        }



        public async Task<Student> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student;
        }

        public IQueryable<Student> GetStudentsQuerable()
        {
            return _studentRepository.GetTableNoTracking().Include(x => x.Department).AsQueryable();
        }




        #endregion
    }



}
