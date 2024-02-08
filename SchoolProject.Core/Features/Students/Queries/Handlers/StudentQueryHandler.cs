using AutoMapper;
using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Queries.Handlers
{
    public class StudentQueryHandler : ResponseHandler, 
        IRequestHandler<GetStudentsListQuery, Response<List<GetStudentsListResponse>>>,
        IRequestHandler<GetStudentByIDQuery, Response<GetSingleStudentResponse>>,
                IRequestHandler<GetStudentPaginatedListQuery, PaginatedResult<GetStudentPaginatedListResponse>>



    {

        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        #endregion
        #region Constructor
        public StudentQueryHandler(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions

        #endregion
        public async Task <Response<List <GetStudentsListResponse>>> Handle (GetStudentsListQuery request , CancellationToken cancellationToken)
        {

            //using the service that will handle the request
            var studentList = await _studentService.GetStudentsListAsync();
            var studentListMapper = _mapper.Map<List<GetStudentsListResponse>>(studentList);
            return Success(studentListMapper);



        }

        public async Task<Response<GetSingleStudentResponse>> Handle (GetStudentByIDQuery request, CancellationToken cancellationToken)
        {

            var student = await _studentService.GetStudentByIDWithIncludeAsync(request.Id);
            if (student == null) return NotFound <GetSingleStudentResponse>("Object not found"); 
            var result = _mapper.Map<GetSingleStudentResponse>(student);
            return Success(result);
        }

        public async Task<PaginatedResult<GetStudentPaginatedListResponse>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Student, GetStudentPaginatedListResponse>> expression = e => new GetStudentPaginatedListResponse(e.StudID, e.Department,e.Department.DName);
            /* var FilterQuery = _studentService.FilterStudentPaginatedQuerable(request.OrderBy, request.Search);*/
            var PaginatedList = await _mapper.ProjectTo<GetStudentPaginatedListResponse>(FilterQuery).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            PaginatedList.Meta = new { Count = PaginatedList.Data.Count() };
            return PaginatedList;
        }
    }

   
}
