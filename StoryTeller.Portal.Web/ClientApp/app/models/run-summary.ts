export class RunSummary {
    public id: number;
    public name: string;
    public appId: number;
    public appName: string;
    public runDateTime: string;
    public passed: boolean;
    public finished: boolean;
    public successfulCount: number;
    public failureCount: number;
    public totalCount: number;
}