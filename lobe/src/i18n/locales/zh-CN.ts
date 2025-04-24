const zhCN = {
  translation: {
    // 通用
    common: {
      search: '搜索',
      operate: '操作',
      edit: '编辑',
      copy: '复制',
      delete: '删除',
      cancel: '取消',
      confirm: '确认',
      submit: '提交',
      success: '成功',
      failed: '失败',
      loading: '加载中',
      noData: '暂无数据',
      yes: '是',
      no: '否',
      create: '创建',
      update: '更新',
      back: '返回',
      next: '下一步',
      copySuccess: '复制成功',
      copyFailed: '复制失败',
      deleteSuccess: '删除成功',
      deleteFailed: '删除失败',
      operateSuccess: '操作成功',
      operateFailed: '操作失败',
      logout: '退出登录',
      community: '社区',
      save: '保存',
      description: '描述',
      name: '名称',
      action: '操作',
      status: '状态',
      time: '时间',
      type: '类型',
      enabled: '已启用',
      disabled: '已禁用',
      enable: '启用',
      disable: '禁用',
      config: '配置',
      add: '添加',
      select: '选择',
      upload: '上传',
      download: '下载',
      view: '查看',
      refresh: '刷新',
      more: '更多',
    },

    // 导航
    nav: {
      panel: '面板',
      channel: '渠道',
      token: 'Token管理',
      modelManager: '模型管理',
      product: '产品',
      logger: '日志',
      redeemCode: '兑换码',
      user: '用户',
      current: '当前',
      setting: '设置',
      rateLimit: '限速',
      modelMap: '模型映射',
      userGroup: '用户分组',
      welcome: '首页',
      doc: '开发文档',
      model: '模型库',
      login: '登录',
      register: '注册',
      auth: '认证',
      enterSystem: '进入系统',
      ai: 'AI服务',
      chat: '对话',
      business: '运营服务',
      wallet: '钱包/个人',
      system: '系统服务',
      apiKeyManage: 'API Key 管理'
    },

    // 侧边栏菜单
    sidebar: {
      panel: '面板',
      channel: '渠道',
      token: 'API Key 管理',
      modelManager: '模型管理',
      product: '产品',
      logger: '日志',
      redeemCode: '兑换码',
      user: '用户管理',
      current: '钱包/个人',
      setting: '系统服务',
      rateLimit: '限流',
      modelMap: '模型映射',
      userGroup: '用户分组',
      ai: 'AI服务',
      chat: '对话',
      business: '运营服务'
    },

    // 页面标题
    pageTitle: {
      loading: {
        panel: '加载面板中',
        channel: '加载渠道中',
        token: '加载Token管理中',
        modelManager: '加载模型管理中',
        product: '加载产品页面中',
        logger: '加载日志页面中',
        redeemCode: '加载兑换码页面中',
        user: '加载用户页面中',
        current: '加载当前页面中',
        setting: '加载设置页面中',
        rateLimit: '加载限速页面中',
        modelMap: '加载模型映射页面中',
        userGroup: '加载用户分组页面中',
        login: '加载登录页面中',
        register: '加载注册页面中',
        auth: '加载认证页面中',
        defaultLayout: '加载默认布局中',
        welcome: '加载欢迎页面中',
        doc: '加载文档页面中',
        model: '加载模型页面中',
      }
    },

    // 登录页面
    login: {
      title: 'TokenAI',
      inputAccountInfo: '输入您的账号信息进行登录',
      accountPlaceholder: '请输入账号',
      accountRequired: '请输入您的账号',
      passwordPlaceholder: '请输入密码',
      passwordRequired: '请输入您的密码',
      loginButton: '登录',
      registerNow: '没有账号？立即注册',
      thirdPartyLogin: '第三方登录',
      githubLogin: 'Github 登录',
      giteeLogin: 'Gitee 登录',
      casdoorLogin: 'Casdoor 登录',
      configGithubClientId: '请联系管理员配置 Github ClientId',
      configGiteeClientId: '请联系管理员配置 Gitee ClientId',
      enableGiteeLogin: '请联系管理员开启 Gitee 登录',
      configCasdoorEndpoint: '请联系管理员配置 Casdoor Endpoint',
      configCasdoorClientId: '请联系管理员配置 Casdoor ClientId',
      loginSuccess: '登录成功，即将跳转到首页',
      loginFailed: '登录失败',
      loginError: '登录过程中出现错误，请重试'
    },

    // 注册页面
    register: {
      title: '注册账号',
      subtitle: '创建一个新账号以访问所有功能',
      usernameLabel: '用户名',
      usernamePlaceholder: '请输入用户名',
      usernameRequired: '请输入用户名',
      emailLabel: '电子邮箱',
      emailPlaceholder: '请输入电子邮箱',
      emailRequired: '请输入电子邮箱',
      emailInvalid: '请输入有效的电子邮箱',
      passwordLabel: '密码',
      passwordPlaceholder: '请输入密码',
      passwordRequired: '请输入密码',
      passwordLength: '密码长度必须至少为6位',
      confirmPasswordLabel: '确认密码',
      confirmPasswordPlaceholder: '请再次输入密码',
      confirmPasswordRequired: '请确认您的密码',
      passwordMismatch: '两次输入的密码不一致',
      verificationCodeLabel: '验证码',
      verificationCodePlaceholder: '请输入验证码',
      getVerificationCode: '获取验证码',
      verificationCodeSent: '验证码已发送',
      resendCode: '重新发送 ({count}s)',
      inviteCodeLabel: '邀请码 (可选)',
      inviteCodePlaceholder: '如果有邀请码请输入',
      registerButton: '注册',
      loginLink: '已有账号？立即登录',
      registerSuccess: '注册成功！即将为您跳转到主页',
      emailNotAllowed: '当前系统不允许使用邮箱进行注册',
      userCreationFailed: '用户创建失败',
      registerError: '注册过程中出现错误，请重试'
    },

    // 首页
    welcome: {
      title: 'Thor 雷神托尔',
      subtitle: '使用标准的OpenAI接口协议访问多种模型',
      description: '使用标准的OpenAI接口协议访问68+模型，不限时间、按量计费、拒绝逆向、极速对话、明细透明，无隐藏消费。',
      tagline: '为您提供最好的AI服务！',
      startNow: '立即开始使用',
      giveStar: '给项目 Star',
      features: {
        title: '我们的优势',
        description: 'Thor雷神托尔为开发者提供一站式AI模型调用服务，简化您的AI应用开发流程',
        multiModel: {
          title: '多模型支持',
          description: '支持200+模型，包括主流的大型语言模型和专业领域模型，满足各种AI应用场景。',
          action: '了解支持的模型'
        },
        fastResponse: {
          title: '高速响应',
          description: '优化的服务架构，确保极速对话体验，减少等待时间，提高工作效率。',
          action: '查看性能测试'
        },
        community: {
          title: '社区驱动',
          description: '由AIDotNet社区维护，持续更新，提供专业的技术支持和丰富的资源分享。',
          action: '加入我们的社区'
        }
      },
      community: {
        title: '强大的社区',
        description: 'Thor由AIDotNet社区维护，社区拥有丰富的AI资源，包括模型、数据集、工具等。',
        payPerUse: {
          title: '按量付费',
          description: '支持用户额度管理，用户可自定义Token 管理，按量计费。'
        },
        appSupport: {
          title: '应用支持',
          description: '支持OpenAi官方库、大部分开源聊天应用、Utools GPT插件'
        },
        transparency: {
          title: '明细可查',
          description: '统计每次请求消耗明细，价格透明，无隐藏消费，用的放心'
        }
      },
      statistics: {
        models: '支持模型',
        users: '社区用户',
        requests: '每日请求量',
        contributors: '代码贡献者'
      },
      modelHot: {
        title: '模型热度排行',
        description: '了解用户最喜爱的AI模型，选择最适合您需求的模型',
        distribution: '模型使用分布',
        distributionDesc: '基于用户实际使用情况的模型热度分布图',
        ranking: '热门模型排行',
        rankingDesc: '最受欢迎的AI模型排名'
      },
      callToAction: {
        title: '准备好开始使用Thor雷神托尔了吗？',
        description: '立即注册并获取免费额度，开始您的AI开发之旅',
        register: '免费注册账号',
        docs: '查看开发文档'
      },
      projects: {
        title: '相关开源项目',
        aidotnet: {
          title: 'AIDotNet',
          description: 'AIDotNet社区是一个热衷于AI开发者组成的社区，旨在推动AI技术的发展，为AI开发者提供更好的学习和交流平台。',
          action: '了解更多'
        },
        fastwiki: {
          title: 'FastWiki',
          description: '一个智能知识库的开源项目，可用于开发企业级智能客服管理系统。支持多种知识库格式，提供高效的检索和答案生成能力。',
          action: '了解更多'
        }
      }
    },

    // 兑换码页面
    redeemCode: {
      title: '兑换码管理',
      name: '名称',
      disabled: '是否禁用',
      quota: '额度',
      createdAt: '创建时间',
      redeemedUser: '兑换人',
      redeemedTime: '兑换时间',
      notRedeemed: '未兑换',
      createRedeemCode: '创建兑换码',
      updateRedeemCode: '更新兑换码',
      batchCreate: '批量创建',
      codeValue: '兑换码值',
      generateCode: '生成兑换码',
      validityPeriod: '有效期',
      days: '天',
      limitQuantity: '限制数量',
      unlimited: '不限制',
      customQuantity: '自定义数量',
      createSuccess: '创建成功',
      createFailed: '创建失败'
    },

    // 用户页面
    user: {
      title: '用户管理',
      username: '用户名',
      email: '电子邮箱',
      roles: '角色',
      status: '状态',
      createdAt: '创建时间',
      lastLogin: '最后登录',
      action: '操作',
      addUser: '添加用户',
      editUser: '编辑用户',
      deleteUser: '删除用户',
      resetPassword: '重置密码',
      enable: '启用',
      disable: '禁用',
      enableSuccess: '用户已启用',
      disableSuccess: '用户已禁用',
      deleteSuccess: '用户已删除',
      resetPasswordSuccess: '密码已重置',
      confirmDelete: '确定要删除该用户吗？',
      confirmResetPassword: '确定要重置该用户的密码吗？'
    },

    // Token管理页面
    token: {
      title: 'Token管理',
      name: '名称',
      token: 'Token值',
      createdAt: '创建时间',
      expiredAt: '过期时间',
      status: '状态',
      createToken: '创建Token',
      editToken: '编辑Token',
      deleteToken: '删除Token',
      regenerateToken: '重新生成Token',
      copyToken: '复制Token',
      neverExpire: '永不过期',
      expired: '已过期',
      active: '活跃',
      inactive: '未激活',
      remaining: '剩余'
    },

    // 模型管理页面
    modelManager: {
      title: '模型管理',
      modelName: '模型名称',
      modelType: '模型类型',
      baseUrl: '基础URL',
      status: '状态',
      apiKey: 'API密钥',
      createModel: '创建模型',
      editModel: '编辑模型',
      deleteModel: '删除模型',
      testConnection: '测试连接',
      connectionSuccess: '连接成功',
      connectionFailed: '连接失败',
      confirmDelete: '确定要删除该模型吗？'
    },

    // 产品页面
    product: {
      title: '产品管理',
      productName: '产品名称',
      productType: '产品类型',
      price: '价格',
      discount: '折扣',
      status: '状态',
      createProduct: '创建产品',
      editProduct: '编辑产品',
      deleteProduct: '删除产品',
      confirmDelete: '确定要删除该产品吗？'
    },

    // 渠道页面
    channel: {
      title: '渠道管理',
      channelName: '渠道名称',
      channelType: '渠道类型',
      apiKey: 'API密钥',
      status: '状态',
      createChannel: '创建渠道',
      editChannel: '编辑渠道',
      deleteChannel: '删除渠道',
      testConnection: '测试连接',
      connectionSuccess: '连接成功',
      connectionFailed: '连接失败',
      confirmDelete: '确定要删除该渠道吗？'
    }
  }
};

export default zhCN; 