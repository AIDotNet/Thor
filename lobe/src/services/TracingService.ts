import { get, del } from "../utils/fetch";
import { message } from "antd";

export interface TracingStatistics {
  totalCount: number;
  todayCount: number;
  yesterdayCount: number;
  weekCount: number;
  oldestRecord?: string;
  newestRecord?: string;
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
}

/**
 * 获取Tracing统计信息
 */
export async function getTracingStatistics(): Promise<ApiResponse<TracingStatistics>> {
  try {
    const data = await get("/api/v1/tracing/statistics");
    return {
      success: true,
      data: data,
    };
  } catch (error: any) {
    message.error(error.message || "获取统计信息失败");
    return {
      success: false,
      data: {
        totalCount: 0,
        todayCount: 0,
        yesterdayCount: 0,
        weekCount: 0,
      },
      message: error.message || "获取统计信息失败",
    };
  }
}

/**
 * 清理过期的Tracing记录
 */
export async function cleanupTracings(retainDays: number = 30): Promise<ApiResponse<{ deletedCount: number; message: string }>> {
  try {
    const data = await del(`/api/v1/tracing/cleanup?retainDays=${retainDays}`);
    return {
      success: true,
      data: data,
    };
  } catch (error: any) {
    message.error(error.message || "清理记录失败");
    return {
      success: false,
      data: { deletedCount: 0, message: "清理失败" },
      message: error.message || "清理记录失败",
    };
  }
}

/**
 * 删除指定日期之前的Tracing记录
 */
export async function deleteTracingsByDate(beforeDate: string): Promise<ApiResponse<{ deletedCount: number; message: string }>> {
  try {
    const data = await del(`/api/v1/tracing/by-date?beforeDate=${encodeURIComponent(beforeDate)}`);
    return {
      success: true,
      data: data,
    };
  } catch (error: any) {
    message.error(error.message || "删除记录失败");
    return {
      success: false,
      data: { deletedCount: 0, message: "删除失败" },
      message: error.message || "删除记录失败",
    };
  }
}

/**
 * 删除指定ChatLoggerId的Tracing记录
 */
export async function deleteTracingsByChatLoggerId(chatLoggerId: string): Promise<ApiResponse<{ deletedCount: number; message: string }>> {
  try {
    const data = await del(`/api/v1/tracing/by-chatloggerid/${encodeURIComponent(chatLoggerId)}`);
    return {
      success: true,
      data: data,
    };
  } catch (error: any) {
    message.error(error.message || "删除记录失败");
    return {
      success: false,
      data: { deletedCount: 0, message: "删除失败" },
      message: error.message || "删除记录失败",
    };
  }
}

/**
 * 删除指定TraceId的Tracing记录
 */
export async function deleteTracingsByTraceId(traceId: string): Promise<ApiResponse<{ deletedCount: number; message: string }>> {
  try {
    const data = await del(`/api/v1/tracing/by-traceid/${encodeURIComponent(traceId)}`);
    return {
      success: true,
      data: data,
    };
  } catch (error: any) {
    message.error(error.message || "删除记录失败");
    return {
      success: false,
      data: { deletedCount: 0, message: "删除失败" },
      message: error.message || "删除记录失败",
    };
  }
}