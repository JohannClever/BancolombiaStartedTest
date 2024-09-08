export interface Report {
    id: number,
    companyServiceName: string,
    companyServiceId: number,
    statusName: string,
    statusId: number,
    observations: string,
    idUser: string,
    userName: string
}

export interface UdateReport {
    id: number,
    companyServiceId: number,
    statusId: number,
    observations: string
}
export interface CreateReport {
    companyServiceId: number,
    statusId: number,
    observations: string,
    idUser: string
}