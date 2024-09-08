export interface Project {
    id: number,
    name: string,
    description: string,
    goal: number,
    pledged: number,
    backersCount: number
    pictureUrl: string,
    userId: string
}

export interface CreateProject {
    name: string;
    description: string;
    goal: number;
    picture?: File; 
}


export interface UdateProject {
    id: number,
    description: string
}