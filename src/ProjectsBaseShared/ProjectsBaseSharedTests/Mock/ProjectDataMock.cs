using System;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Mock
{
    internal class ProjectDataMock
    {
        public Guid ProjectId => Project.ProjectId;
        public Project Project { get; set; }
        public readonly string ProjectName = "Test project";
        public readonly DateTime ProjectStartDate = DateTime.Now;
        public readonly DateTime ProjectEndDate = DateTime.Now.AddDays(7);

        public ProjectDataMock()
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            Project = new Project()
            {
                ProjectName = ProjectName,
                ProjectStartDate = ProjectStartDate,
                ProjectEndDate = ProjectEndDate,
                
            };
            Project.Client = new ClientDataMock(this.Project).Client; //must be outside of Project initializer or this will be null
            var auditTeamMember = new AuditTeam() { Auditor = new AuditorDataMock(this).Auditor };
            Project.Auditors.Add(auditTeamMember);
            Project.Auditors.Add(auditTeamMember);
        }

        public ProjectDataMock(ClientDataMock clientData)
        {
            Project = new Project
            {
                ProjectName = ProjectName,
                ProjectStartDate = ProjectStartDate,
                ProjectEndDate = ProjectEndDate,
                Client = clientData.Client,
            };
            var auditTeamMember = new AuditTeam() { Auditor = new AuditorDataMock(this).Auditor };
            Project.Auditors.Add(auditTeamMember);
            Project.Auditors.Add(auditTeamMember);
        }

        public ProjectDataMock(AuditorDataMock auditorDataMock)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            Project = new Project()
            {
                ProjectName = ProjectName,
                ProjectStartDate = ProjectStartDate,
                ProjectEndDate = ProjectEndDate,

            };
            Project.Client = new ClientDataMock(this.Project).Client; //must be outside of Project initializer or this will be null
            var auditTeamMember = new AuditTeam() { Auditor = auditorDataMock.Auditor };
            Project.Auditors.Add(auditTeamMember);
            Project.Auditors.Add(auditTeamMember);
        }
    }
}
