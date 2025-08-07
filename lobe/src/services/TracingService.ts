import axios from "axios";
import { message } from "antd";

const api = axios.create({
  timeout: 120000,
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => {
    if (response.data.success === false) {
      message.error(response.data.message || "请求失败");
      return Promise.reject(new Error(response.data.message || "请求失败"));
    }
    return response;
  },
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      window.location.href = "/login";
      return Promise.reject(error);
    }
    
    const errorMessage = error.response?.data?.message || error.message || "请求失败";
    message.error(errorMessage);
    return Promise.reject(error);
  }
);

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
    const response = await api.get("/api/v1/tracing/statistics");
    return {
      success: true,
      data: response.data,
    };
  } catch (error: any) {
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
    const response = await api.delete(`/api/v1/tracing/cleanup?retainDays=${retainDays}`);
    return {
      success: true,
      data: response.data,
    };
  } catch (error: any) {
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
    const response = await api.delete(`/api/v1/tracing/by-date?beforeDate=${encodeURIComponent(beforeDate)}`);
    return {
      success: true,
      data: response.data,
    };
  } catch (error: any) {
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
    const response = await api.delete(`/api/v1/tracing/by-chatloggerid/${encodeURIComponent(chatLoggerId)}`);
    return {
      success: true,
      data: response.data,
    };
  } catch (error: any) {
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
    const response = await api.delete(`/api/v1/tracing/by-traceid/${encodeURIComponent(traceId)}`);
    return {
      success: true,
      data: response.data,
    };
  } catch (error: any) {
    return {
      success: false,
      data: { deletedCount: 0, message: "删除失败" },
      message: error.message || "删除记录失败",
    };
  }
}