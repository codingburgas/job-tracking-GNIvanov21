const API_BASE_URL = 'http://localhost:5230';

document.addEventListener('DOMContentLoaded', () => {

    // --- Logic for index.html (Login/Register Page) ---
    if (document.getElementById('login-form')) {
        const loginForm = document.getElementById('login-form');
        const registerForm = document.getElementById('register-form');
        const loginContainer = document.getElementById('login-form-container');
        const registerContainer = document.getElementById('register-form-container');
        const showRegisterLink = document.getElementById('show-register');
        const showLoginLink = document.getElementById('show-login');
        const loginError = document.getElementById('login-error');
        const registerError = document.getElementById('register-error');

        showRegisterLink.addEventListener('click', (e) => {
            e.preventDefault();
            loginContainer.style.display = 'none';
            registerContainer.style.display = 'block';
        });

        showLoginLink.addEventListener('click', (e) => {
            e.preventDefault();
            registerContainer.style.display = 'none';
            loginContainer.style.display = 'block';
        });

        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            loginError.textContent = '';
            const username = document.getElementById('login-username').value;
            const password = document.getElementById('login-password').value;
            try {
                const response = await fetch(`${API_BASE_URL}/api/auth/login`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ username, password })
                });
                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.message || 'Invalid credentials');
                }
                const data = await response.json();
                localStorage.setItem('user', JSON.stringify(data));
                window.location.href = 'jobs.html'; // Redirect to jobs.html after login
            } catch (error) {
                loginError.textContent = error.message;
            }
        });

        registerForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            registerError.textContent = '';
            const firstName = document.getElementById('reg-firstname').value;
            const lastName = document.getElementById('reg-lastname').value;
            const username = document.getElementById('reg-username').value;
            const password = document.getElementById('reg-password').value;
            try {
                const response = await fetch(`${API_BASE_URL}/api/auth/register`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ firstName, lastName, username, password, middleName: "" })
                });
                if (!response.ok) {
                    const errorText = await response.text();
                    throw new Error(errorText || 'Registration failed');
                }
                alert('Registration successful! Please log in.');
                registerForm.reset();
                showLoginLink.click();
            } catch (error) {
                registerError.textContent = error.message;
            }
        });
    }

    // --- Logic for jobs.html page (User and Admin) ---
    if (window.location.pathname.endsWith('jobs.html')) {
        const user = JSON.parse(localStorage.getItem('user'));
        if (!user) {
            window.location.href = 'index.html'; // Redirect if not logged in
            return;
        }

        const welcomeMessage = document.getElementById('welcome-message');
        const jobsListContainer = document.getElementById('jobs-list-container');
        const logoutButton = document.getElementById('logout-button');
        const showAppliedJobsButton = document.getElementById('show-applied-jobs-button');
        const appliedJobsContainer = document.getElementById('applied-jobs-container');
        const appliedJobsList = document.getElementById('applied-jobs-list');
        const appliedJobsError = document.getElementById('applied-jobs-error');

        // Admin specific elements
        const adminPanel = document.getElementById('admin-panel');
        const createJobForm = document.getElementById('create-job-form');
        const adminJobsListContainer = document.getElementById('admin-jobs-list-container');
        const jobError = document.getElementById('job-error');
        const applicationsMailbox = document.getElementById('applications-mailbox'); // For admin to see applications

        welcomeMessage.textContent = `Welcome, ${user.username}!`;

        logoutButton.addEventListener('click', () => {
            localStorage.removeItem('user');
            window.location.href = 'index.html';
        });

        // --- User Functionality ---

        async function loadJobs() {
            try {
                const response = await fetch(`${API_BASE_URL}/api/jobs`);
                if (!response.ok) throw new Error('Could not fetch jobs.');
                const jobs = await response.json();
                jobsListContainer.innerHTML = '';
                if (jobs.length === 0) {
                    jobsListContainer.innerHTML = '<p>No jobs available at the moment.</p>';
                    return;
                }
                jobs.forEach(job => {
                    const jobCard = document.createElement('div');
                    jobCard.className = 'job-card';
                    jobCard.innerHTML = `
                        <h4>${job.title}</h4>
                        <p><strong>Company ID:</strong> ${job.companyId}</p>
                        <p>${job.description}</p>
                        <small>Posted on: ${new Date(job.publicationDate).toLocaleDateString()}</small>
                        <br>
                        <button class="apply-button" data-job-id="${job.id}">Apply</button>
                    `;
                    jobsListContainer.appendChild(jobCard);
                });

                document.querySelectorAll('.apply-button').forEach(button => {
                    button.addEventListener('click', async (e) => {
                        const jobId = e.target.dataset.jobId;
                        await applyForJob(jobId);
                    });
                });

            } catch (error) {
                jobsListContainer.innerHTML = `<p class="error-message">${error.message}</p>`;
            }
        }

        async function applyForJob(jobId) {
            if (!user || !user.token) {
                alert('You need to be logged in to apply for a job.');
                window.location.href = 'index.html';
                return;
            }

            try {
                const response = await fetch(`${API_BASE_URL}/api/jobs/${jobId}/apply`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${user.token}`
                    },
                    body: JSON.stringify({ userId: user.id })
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.message || 'Failed to apply for job.');
                }

                alert('Application submitted successfully!');
                loadAppliedJobs(); // Refresh applied jobs list
            } catch (error) {
                alert(`Error applying for job: ${error.message}`);
            }
        }

        async function loadAppliedJobs() {
            appliedJobsError.textContent = ''; // Clear previous errors
            if (!user || !user.token) {
                appliedJobsList.innerHTML = '<p>Please log in to view your applications.</p>';
                return;
            }

            try {
                const response = await fetch(`${API_BASE_URL}/api/applications/user/${user.id}`, {
                    headers: {
                        'Authorization': `Bearer ${user.token}`
                    }
                });
                if (!response.ok) throw new Error('Could not fetch applied jobs.');
                const appliedJobs = await response.json();

                appliedJobsList.innerHTML = ''; // Clear previous content
                if (appliedJobs.length === 0) {
                    appliedJobsList.innerHTML = '<p>You have not applied for any jobs yet.</p>';
                    return;
                }

                appliedJobs.forEach(application => {
                    const appliedJobCard = document.createElement('div');
                    appliedJobCard.className = 'job-card applied-job-card';
                    appliedJobCard.innerHTML = `
                        <h4>${application.jobTitle}</h4>
                        <p><strong>Company:</strong> ${application.companyName || 'N/A'}</p>
                        <p><strong>Status:</strong> ${application.status}</p>
                        <small>Applied on: ${new Date(application.applicationDate).toLocaleDateString()}</small>
                    `;
                    appliedJobsList.appendChild(appliedJobCard);
                });
            } catch (error) {
                appliedJobsError.textContent = `Error loading applied jobs: ${error.message}`;
                appliedJobsList.innerHTML = ''; // Clear content on error
            }
        }

        showAppliedJobsButton.addEventListener('click', () => {
            if (appliedJobsContainer.style.display === 'none' || appliedJobsContainer.style.display === '') {
                appliedJobsContainer.style.display = 'block';
                showAppliedJobsButton.textContent = 'Hide My Applications';
                loadAppliedJobs(); // Load applications when shown
            } else {
                appliedJobsContainer.style.display = 'none';
                showAppliedJobsButton.textContent = 'Show My Applications';
            }
        });

        // --- Admin Functionality ---

        // Display admin panel if user is an admin (assuming 'role' in user object)
        if (user.role === 'Admin') {
            adminPanel.style.display = 'block';
            loadAdminJobs();
            loadApplicationsMailbox(); // Load mailbox for admin
        }

        async function loadAdminJobs() {
            try {
                const response = await fetch(`${API_BASE_URL}/api/jobs/company/${user.companyId}`, { // Assuming admin sees jobs for their company
                    headers: {
                        'Authorization': `Bearer ${user.token}`
                    }
                });
                if (!response.ok) throw new Error('Could not fetch jobs for admin.');
                const jobs = await response.json();
                adminJobsListContainer.innerHTML = '';
                if (jobs.length === 0) {
                    adminJobsListContainer.innerHTML = '<p>No jobs posted by your company yet.</p>';
                    return;
                }
                jobs.forEach(job => {
                    const jobCard = document.createElement('div');
                    jobCard.className = 'job-card';
                    jobCard.innerHTML = `
                        <h4>${job.title}</h4>
                        <p><strong>Company ID:</strong> ${job.companyId}</p>
                        <p>${job.description}</p>
                        <small>Posted on: ${new Date(job.publicationDate).toLocaleDateString()}</small>
                        <br>
                        <button class="delete-job-button" data-job-id="${job.id}">Delete</button>
                    `;
                    adminJobsListContainer.appendChild(jobCard);
                });

                document.querySelectorAll('.delete-job-button').forEach(button => {
                    button.addEventListener('click', async (e) => {
                        const jobId = e.target.dataset.jobId;
                        if (confirm('Are you sure you want to delete this job?')) {
                            await deleteJob(jobId);
                        }
                    });
                });

            } catch (error) {
                adminJobsListContainer.innerHTML = `<p class="error-message">${error.message}</p>`;
            }
        }

        async function deleteJob(jobId) {
            try {
                const response = await fetch(`${API_BASE_URL}/api/jobs/${jobId}`, {
                    method: 'DELETE',
                    headers: {
                        'Authorization': `Bearer ${user.token}`
                    }
                });
                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.message || 'Failed to delete job.');
                }
                alert('Job deleted successfully!');
                loadAdminJobs();
                loadJobs(); // Refresh public job list as well
            } catch (error) {
                alert(`Error deleting job: ${error.message}`);
            }
        }

        async function loadApplicationsMailbox() {
            // This function would fetch applications relevant to the admin's company.
            // You'll need a backend endpoint like /api/applications/company/{companyId}
            // and potentially update/approve/reject functionalities for each application.
            applicationsMailbox.innerHTML = '<p>Loading applications for your company...</p>';
            try {
                const response = await fetch(`${API_BASE_URL}/api/applications/company/${user.companyId}`, {
                    headers: {
                        'Authorization': `Bearer ${user.token}`
                    }
                });
                if (!response.ok) throw new Error('Could not fetch applications mailbox.');
                const applications = await response.json();

                applicationsMailbox.innerHTML = '';
                if (applications.length === 0) {
                    applicationsMailbox.innerHTML = '<p>No new applications for your company.</p>';
                    return;
                }

                applications.forEach(app => {
                    const appCard = document.createElement('div');
                    appCard.className = 'job-card application-card'; // Add specific class
                    appCard.innerHTML = `
                        <h4>Application for: ${app.jobTitle}</h4>
                        <p><strong>Applicant Username:</strong> ${app.applicantUsername}</p>
                        <p><strong>Applicant Name:</strong> ${app.applicantFirstName} ${app.applicantLastName}</p>
                        <p><strong>Status:</strong> <span class="application-status-${app.status.toLowerCase()}">${app.status}</span></p>
                        <small>Applied on: ${new Date(app.applicationDate).toLocaleDateString()}</small>
                        <br>
                        <button class="status-button approve-button" data-application-id="${app.id}" data-status="Approved">Approve</button>
                        <button class="status-button reject-button" data-application-id="${app.id}" data-status="Rejected">Reject</button>
                    `;
                    applicationsMailbox.appendChild(appCard);
                });

                document.querySelectorAll('.status-button').forEach(button => {
                    button.addEventListener('click', async (e) => {
                        const applicationId = e.target.dataset.applicationId;
                        const newStatus = e.target.dataset.status;
                        await updateApplicationStatus(applicationId, newStatus);
                    });
                });

            } catch (error) {
                applicationsMailbox.innerHTML = `<p class="error-message">Error loading applications mailbox: ${error.message}</p>`;
            }
        }

        async function updateApplicationStatus(applicationId, status) {
            try {
                const response = await fetch(`${API_BASE_URL}/api/applications/${applicationId}/status`, {
                    method: 'PUT', // or PATCH depending on your API
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${user.token}`
                    },
                    body: JSON.stringify({ status: status })
                });

                if (!response.ok) {
                    const errorData = await response.json();
                    throw new Error(errorData.message || `Failed to update application status to ${status}.`);
                }
                alert(`Application status updated to ${status}!`);
                loadApplicationsMailbox(); // Reload mailbox
            } catch (error) {
                alert(`Error updating application status: ${error.message}`);
            }
        }

        createJobForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            jobError.textContent = '';
            const title = document.getElementById('job-title').value;
            const companyId = document.getElementById('job-company-id').value; // Corrected ID
            const description = document.getElementById('job-description').value;

            try {
                const response = await fetch(`${API_BASE_URL}/api/jobs`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${user.token}`
                    },
                    body: JSON.stringify({ title, companyId: parseInt(companyId), description })
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    throw new Error(errorText || 'Failed to add job.');
                }
                alert('Job added successfully!');
                createJobForm.reset();
                loadAdminJobs();
                loadJobs(); // Also refresh the public list
            } catch (error) {
                jobError.textContent = error.message;
            }
        });

        // Initial load of jobs when jobs.html loads (for all users)
        loadJobs();
    }
});