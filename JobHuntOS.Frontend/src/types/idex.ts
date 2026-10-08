export enum ApplicationStatus {
    Applied = 'Applied',
    Interview = 'Interview',
    Offer = 'Offer',
    Rejected = 'Rejected',
    Withdrawn = 'Withdrawn'
}

export interface JobApplication {
    id: string;
    companyName: string;
    jobTitle: string;
    jobUrl?: string;
    jobDescription?: string;
    status: ApplicationStatus;
    location?: string;
    isRemote: boolean;
    salaryMin?: number;
    salaryMax?: number;
    appliedDate: string;
    followUpDate?: string;
    createdAt: string;
    lastUpdated: string;
}

export interface CreateApplicationDto {
    companyName: string;
    jobTitle: string;
    jobUrl?: string;
    jobDescription?: string;
    location?: string;
    isRemote: boolean;
    salaryMin?: number;
    salaryMax?: number;
    appliedDate: string;
    followUpDays: number;
}

export interface UpdateApplicationDto {
    companyName?: string;
    jobTitle?: string;
    jobUrl?: string;
    jobDescription?: string;
    status?: ApplicationStatus;
    location?: string;
    isRemote?: boolean;
    salaryMin?: number;
    salaryMax?: number;
    followUpDate?: string;
}

export interface Note {
    id: string;
    applicationId: string;
    content: string;
    createdAt: string;
}

export interface CreateNoteDto {
    content: string;
}

export interface Cv {
    id: string;
    content: string;
    fileName?: string;
    uploadedAt: string;
}

export interface UpsertCvDto {
    content: string;
    fileName?: string;
}

export interface AnalysisRequest {
    jobDescription: string;
    applicationId?: string;
}

export interface AnalysisResponse {
    id: string;
    applicationId?: string;
    fitScore: number;
    matchingSkills: string[];
    missingSkills: string[];
    recommendations?: string;
    analysedAt: string;
}

export interface DashboardStats {
    totalApplications: number;
    applied: number;
    interviews: number;
    offers: number;
    rejected: number;
    withdrawn: number;
    overdueFollowUps: number;
    responseRate: number;
}