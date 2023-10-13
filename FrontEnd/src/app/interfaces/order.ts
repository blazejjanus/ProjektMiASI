export interface IOrder {
    cancelationTime: string;
    car: {
        brand: string;
        engineCapacity: number;
        fuelType: number;
        horsepower: number;
        id: number;
        isDeleted: boolean;
        isOperational: boolean;
        longDescription: string;
        model: string;
        pricePerDay: number;
        productionYear: number;
        registrationNumber: string;
        seatsNumber: number;
        shortDescription: string;
    };
    customer: {
        address: {
            street: string;
            houseNumber: string;
            postalCode: string;
            city: string;
        };
        email: string;
        id: number;
        isDeleted: boolean;
        name: string;
        password: string;
        phoneNumber: string;
        surname: string;
        userType: number;
    };
    id: number;
    rentEnd: string;
    rentStart: string;
}