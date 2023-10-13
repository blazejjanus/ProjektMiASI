export interface CarInfo {
    id: number;
    brand: string;
    model: string;
    fuelType: number;
    horsepower: number;
    productionYear: number;
    engineCapacity: number;
    shortDescription: string;
    longDescription: string;
    seatsNumber: number;
    registrationNumber: string;
    productionDate: string;
    pricePerDay: number;
    mainPhoto: string;
    photos: any
}

export interface IPhotos {
    id: number,
    photo: string
}