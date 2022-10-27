using GSG.Model.DTO.Requests;
using GSG.Model.DTO.Responses;
using GSG.Shared;
using GSG.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace GSG.WebApp.Client.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient client;
        private readonly IConfiguration config;
        private string errorMessage;
        public EmployeeService(HttpClient Client, IConfiguration Config)
        {
            client = Client;
            config = Config;
        }

        #region Employee

        public async Task<ResponseBody<List<EmployeeGridResponse>>> GetEmployeesFull()
        {
            return await client.GetFromJsonAsync<ResponseBody<List<EmployeeGridResponse>>>(config["GsgApi:getEmployeesfull"]);
        }

        public async Task<ResponseBody<EmployeeResponse>> GetEmployeeById(int Id)
        {
            return await client.GetFromJsonAsync<ResponseBody<EmployeeResponse>>(config["GsgApi:getEmployeeById"] + Id);
        }

        public async Task<ResponseBody<EmployeeRequest>> CreateNewEmployee(EmployeeRequest empRequest)
        {
            try
            {
                var response = await client.PostAsJsonAsync(config["GsgApi:createNewEmployee"], empRequest);

                errorMessage = response.ReasonPhrase;

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseBody<EmployeeRequest>
                    {
                        Body = null,
                        ReponseCode = (int)response.StatusCode,
                        Success = false,
                        Message = $"Error Adding Employee | {errorMessage}"
                    };
                }

                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<ResponseBody<EmployeeRequest>>(apiResponse);

                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{errorMessage} | {ex.Message}");
                throw new Exception(errorMessage, ex);

            }
        }

        public async Task<ResponseBody<EmployeeResponse>> GetEmployeeToUpdate(int empId)
        {
            return await client.GetFromJsonAsync<ResponseBody<EmployeeResponse>>(config["GsgApi:getEmployeeToUpdate"] + empId);
        }

        public async Task<ResponseBody<EmployeeResponse>> UpdateEmployee(int empId, EmployeeRequest updateEmpRequest)
        {
            try
            {
                var response = await client.PutAsJsonAsync(config["GsgApi:updateEmployee"] + empId, updateEmpRequest);

                errorMessage = response.ReasonPhrase;

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseBody<EmployeeResponse>
                    {
                        Body = null,
                        ReponseCode = (int)response.StatusCode,
                        Success = false,
                        Message = $"Error Updating Employee | {errorMessage}"
                    };
                }

                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadAsStringAsync();
                var UpdatedEmployee = JsonConvert.DeserializeObject<ResponseBody<EmployeeResponse>>(apiResponse);

                return UpdatedEmployee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{errorMessage} | {ex.Message}");
                throw new Exception(errorMessage, ex);

            }
        }


        #endregion


        #region Certificate

        public async Task<ResponseBody<List<CertificateResponse>>> GetCertificates()
        {
            return await client.GetFromJsonAsync<ResponseBody<List<CertificateResponse>>>(config["GsgApi:getCertificates"]);
        }

        public async Task<ResponseBody<CertificateResponse>> GetCertificateToAdd(int certificateId)
        {
            return await client.GetFromJsonAsync<ResponseBody<CertificateResponse>>(config["GsgApi:getCertToAdd"] + certificateId);
        }

        public async Task<ResponseBody<List<EmployeeCertificateProfileResponse>>> GetEmployeeCertificates(string url)
        {
            return await client.GetFromJsonAsync<ResponseBody<List<EmployeeCertificateProfileResponse>>>($"{config["GsgApi:getEmployeeCertificates"]}{url}");
        }

        public async Task<ResponseBody<EmployeeCertificateResponse>> CreateEmployeeCertificate(EmployeeCertificateRequest empCert)
        {
            try
            {
                var response = await client.PostAsJsonAsync(config["GsgApi:createEmployeeCertificate"], empCert);

                errorMessage = response.ReasonPhrase;

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseBody<EmployeeCertificateResponse>
                    {
                        Body = null,
                        ReponseCode = (int)response.StatusCode,
                        Success = false,
                        Message = $"Error Adding Certificate | {errorMessage}"
                    };
                }

                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadAsStringAsync();
                var employeeCerificate = JsonConvert.DeserializeObject<ResponseBody<EmployeeCertificateResponse>>(apiResponse);

                return employeeCerificate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{errorMessage} | {ex.Message}");
                throw new Exception(errorMessage, ex);

            }

        }

        public async Task<string> CreateEmpCertificate(EmployeeCertificateRequest empCert)
        {
            try
            {
                var response = await client.PostAsJsonAsync(config["GsgApi:createEmpCertificate"], empCert);
                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadAsStringAsync();
                var employeeCerificate = JsonConvert.DeserializeObject<string>(apiResponse);

                errorMessage = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    return $"Error adding employee certificate | {errorMessage} | {response.StatusCode} ";
                }

                return employeeCerificate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{errorMessage} | {ex.Message}");
                throw new Exception(errorMessage, ex);

            }

        }

        public Task<ResponseBody<EmployeeCertificateResponse>> GetEmpCertToDelete(int certificateId)
        {
            return client.GetFromJsonAsync<ResponseBody<EmployeeCertificateResponse>>(config["GsgApi:getEmpCertToDelete"] + certificateId);
        }

        public async Task<ResponseBody<string>> DeleteEmployeeCertificate(int certId, int empId)
        {
            try
            {
                var response = await client.DeleteAsync($"{config["GsgApi:deleteEmployeeCert"]}{certId}/{empId}");

                errorMessage = response.ReasonPhrase;

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = $"Error Deleting Certificate | {response.StatusCode} | {response.ReasonPhrase}";

                    return new ResponseBody<string>
                    {
                        Body = null,
                        ReponseCode = (int)response.StatusCode,
                        Success = false,
                        Message = $"Error Adding Certificate | {errorMessage}"
                    };
                }

                var apiResponse = await response.Content.ReadAsStringAsync();
                var employeeCert = JsonConvert.DeserializeObject<ResponseBody<string>>(apiResponse);

                return employeeCert;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{errorMessage} | {ex.Message}");
                throw new Exception(errorMessage, ex);

            }

        }


        #endregion


        #region Skill

        public async Task<ResponseBody<List<SkillResponse>>> GetSkills()
        {
            return await client.GetFromJsonAsync<ResponseBody<List<SkillResponse>>>(config["GsgApi:getSkills"]);
        }

        public async Task<ResponseBody<SkillResponse>> GetSkillToAdd(int skillId)
        {
            return await client.GetFromJsonAsync<ResponseBody<SkillResponse>>(config["GsgApi:getSkillToAdd"] + skillId);
        }

        public async Task<ResponseBody<List<EmployeeSkillProfileResponse>>>GetEmployeeSkills(string url)
        {
            return await client.GetFromJsonAsync<ResponseBody<List<EmployeeSkillProfileResponse>>>($"{config["GsgApi:getEmployeeSkills"]}{url}");
        }

        public async Task<ResponseBody<EmployeeSkillResponse>> CreateEmployeeSkill(EmployeeSkillRequest empSkill)
        {
            try
            {
                var response = await client.PostAsJsonAsync(config["GsgApi:createEmployeeSkill"], empSkill);

                errorMessage = response.ReasonPhrase;

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseBody<EmployeeSkillResponse>
                    {
                        Body = null,
                        ReponseCode = (int)response.StatusCode,
                        Success = false,
                        Message = $"Error Adding Skill | {errorMessage}"
                    };
                }

                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadAsStringAsync();
                var employeeSkill = JsonConvert.DeserializeObject<ResponseBody<EmployeeSkillResponse>>(apiResponse);

                return employeeSkill;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{errorMessage} | {ex.Message}");
                throw new Exception(errorMessage, ex);

            }

        }

        public Task<ResponseBody<EmployeeSkillResponse>>GetEmpSkillToDelete(int skillId)
        {
            return client.GetFromJsonAsync<ResponseBody<EmployeeSkillResponse>>(config["GsgApi:getEmpSkillToDelete"] + skillId);
        }

        public async Task<ResponseBody<string>> DeleteEmployeeSkill(int skillId, int empId)
        {
            try
            {
                var response = await client.DeleteAsync($"{config["GsgApi:deleteEmployeeSkill"]}{skillId}/{empId}");

                errorMessage = response.ReasonPhrase;

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = $"Error Deleting Skill | {response.StatusCode} | {response.ReasonPhrase}";

                    return new ResponseBody<string>
                    {
                        Body = null,
                        ReponseCode = (int)response.StatusCode,
                        Success = false,
                        Message = $"Error Deleting Skill | {errorMessage}"
                    };
                }

                var apiResponse = await response.Content.ReadAsStringAsync();
                var employeeSkill = JsonConvert.DeserializeObject<ResponseBody<string>>(apiResponse);

                return employeeSkill;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{errorMessage} | {ex.Message}");
                throw new Exception(errorMessage, ex);

            }

        }


        #endregion


        #region Role

        public async Task<ResponseBody<List<RoleResponse>>> GetRoles()
        {
            return await client.GetFromJsonAsync<ResponseBody<List<RoleResponse>>>(config["GsgApi:getRoles"]);
        }

        #endregion


        #region Project

        public async Task<ResponseBody<List<ProjectResponse>>> GetProjects()
        {
            return await client.GetFromJsonAsync<ResponseBody<List<ProjectResponse>>>(config["GsgApi:getProjects"]); // GetSkillToAdd
        }

        


        #endregion

















    }
}
