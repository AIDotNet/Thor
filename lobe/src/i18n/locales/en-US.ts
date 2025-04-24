const enUS = {
  translation: {
    // Common
    common: {
      search: 'Search',
      operate: 'Operate',
      edit: 'Edit',
      copy: 'Copy',
      delete: 'Delete',
      cancel: 'Cancel',
      confirm: 'Confirm',
      submit: 'Submit',
      success: 'Success',
      failed: 'Failed',
      loading: 'Loading',
      noData: 'No Data',
      yes: 'Yes',
      no: 'No',
      create: 'Create',
      update: 'Update',
      back: 'Back',
      next: 'Next',
      copySuccess: 'Copy Success',
      copyFailed: 'Copy Failed',
      deleteSuccess: 'Delete Success',
      deleteFailed: 'Delete Failed',
      operateSuccess: 'Operation Successful',
      operateFailed: 'Operation Failed',
      logout: 'Logout',
      community: 'Community',
      save: 'Save',
      description: 'Description',
      name: 'Name',
      action: 'Action',
      status: 'Status',
      time: 'Time',
      type: 'Type',
      enabled: 'Enabled',
      disabled: 'Disabled',
      enable: 'Enable',
      disable: 'Disable',
      config: 'Config',
      add: 'Add',
      select: 'Select',
      upload: 'Upload',
      download: 'Download',
      view: 'View',
      refresh: 'Refresh',
      more: 'More',
    },

    // Navigation
    nav: {
      panel: 'Panel',
      channel: 'Channel',
      token: 'Token Management',
      modelManager: 'Model Manager',
      product: 'Product',
      logger: 'Logger',
      redeemCode: 'Redeem Code',
      user: 'User',
      current: 'Current',
      setting: 'Setting',
      rateLimit: 'Rate Limit',
      modelMap: 'Model Map',
      userGroup: 'User Group',
      welcome: 'Home',
      doc: 'Documentation',
      model: 'Model Library',
      login: 'Login',
      register: 'Register',
      auth: 'Authentication',
      enterSystem: 'Enter System',
      ai: 'AI Service',
      chat: 'Chat',
      business: 'Business Service',
      wallet: 'Wallet/Personal',
      system: 'System Service',
      apiKeyManage: 'API Key Management'
    },

    // Sidebar Menu
    sidebar: {
      panel: 'Panel',
      channel: 'Channel',
      token: 'API Key Management',
      modelManager: 'Model Manager',
      product: 'Product',
      logger: 'Logger',
      redeemCode: 'Redeem Code',
      user: 'User Management',
      current: 'Wallet/Personal',
      setting: 'System Service',
      rateLimit: 'Rate Limit',
      modelMap: 'Model Map',
      userGroup: 'User Group',
      ai: 'AI Service',
      chat: 'Chat',
      business: 'Business Service'
    },

    // Page Title
    pageTitle: {
      loading: {
        panel: 'Loading Panel',
        channel: 'Loading Channel',
        token: 'Loading Token Management',
        modelManager: 'Loading Model Manager',
        product: 'Loading Product',
        logger: 'Loading Logger',
        redeemCode: 'Loading Redeem Code',
        user: 'Loading User',
        current: 'Loading Current',
        setting: 'Loading Setting',
        rateLimit: 'Loading Rate Limit',
        modelMap: 'Loading Model Map',
        userGroup: 'Loading User Group',
        login: 'Loading Login',
        register: 'Loading Register',
        auth: 'Loading Authentication',
        defaultLayout: 'Loading Default Layout',
        welcome: 'Loading Welcome Page',
        doc: 'Loading Documentation',
        model: 'Loading Model Page',
      }
    },

    // Login Page
    login: {
      title: 'TokenAI',
      inputAccountInfo: 'Enter your account information to login',
      accountPlaceholder: 'Please enter account',
      accountRequired: 'Please enter your account',
      passwordPlaceholder: 'Please enter password',
      passwordRequired: 'Please enter your password',
      loginButton: 'Login',
      registerNow: 'No account? Register now',
      thirdPartyLogin: 'Third party login',
      githubLogin: 'Github Login',
      giteeLogin: 'Gitee Login',
      casdoorLogin: 'Casdoor Login',
      configGithubClientId: 'Please contact administrator to configure Github ClientId',
      configGiteeClientId: 'Please contact administrator to configure Gitee ClientId',
      enableGiteeLogin: 'Please contact administrator to enable Gitee login',
      configCasdoorEndpoint: 'Please contact administrator to configure Casdoor Endpoint',
      configCasdoorClientId: 'Please contact administrator to configure Casdoor ClientId',
      loginSuccess: 'Login successful, redirecting to homepage',
      loginFailed: 'Login failed',
      loginError: 'Error occurred during login, please try again'
    },

    // Register Page
    register: {
      title: 'Register Account',
      subtitle: 'Create a new account to access all features',
      usernameLabel: 'Username',
      usernamePlaceholder: 'Please enter username',
      usernameRequired: 'Please enter username',
      emailLabel: 'Email',
      emailPlaceholder: 'Please enter email',
      emailRequired: 'Please enter email',
      emailInvalid: 'Please enter a valid email',
      passwordLabel: 'Password',
      passwordPlaceholder: 'Please enter password',
      passwordRequired: 'Please enter password',
      passwordLength: 'Password must be at least 6 characters',
      confirmPasswordLabel: 'Confirm Password',
      confirmPasswordPlaceholder: 'Please confirm your password',
      confirmPasswordRequired: 'Please confirm your password',
      passwordMismatch: 'The two passwords do not match',
      verificationCodeLabel: 'Verification Code',
      verificationCodePlaceholder: 'Please enter verification code',
      getVerificationCode: 'Get Code',
      verificationCodeSent: 'Verification code sent',
      resendCode: 'Resend ({count}s)',
      inviteCodeLabel: 'Invitation Code (Optional)',
      inviteCodePlaceholder: 'Enter invitation code if you have one',
      registerButton: 'Register',
      loginLink: 'Already have an account? Login now',
      registerSuccess: 'Registration successful! Redirecting to homepage',
      emailNotAllowed: 'The system does not allow registration with email',
      userCreationFailed: 'User creation failed',
      registerError: 'Error occurred during registration, please try again'
    },

    // Home Page
    welcome: {
      title: 'Thor',
      subtitle: 'Access multiple models with standard OpenAI interface protocol',
      description: 'Access 68+ models with standard OpenAI interface protocol, no time limit, pay-per-use, no reverse engineering, fast dialogue, transparent details, no hidden costs.',
      tagline: 'Providing you with the best AI service!',
      startNow: 'Start Now',
      giveStar: 'Star the Project',
      features: {
        title: 'Our Advantages',
        description: 'Thor provides developers with one-stop AI model calling service, simplifying your AI application development process',
        multiModel: {
          title: 'Multi-Model Support',
          description: 'Support 200+ models, including mainstream large language models and professional domain models, to meet various AI application scenarios.',
          action: 'Learn about supported models'
        },
        fastResponse: {
          title: 'Fast Response',
          description: 'Optimized service architecture ensures high-speed conversation experience, reduces waiting time, and improves work efficiency.',
          action: 'View performance tests'
        },
        community: {
          title: 'Community Driven',
          description: 'Maintained by the AIDotNet community, continuously updated, providing professional technical support and rich resource sharing.',
          action: 'Join our community'
        }
      },
      community: {
        title: 'Powerful Community',
        description: 'Thor is maintained by the AIDotNet community, which has rich AI resources including models, datasets, tools, etc.',
        payPerUse: {
          title: 'Pay Per Use',
          description: 'Support user quota management, users can customize Token management, pay by usage.'
        },
        appSupport: {
          title: 'Application Support',
          description: 'Support for OpenAI official libraries, most open-source chat applications, Utools GPT plugin'
        },
        transparency: {
          title: 'Transparent Details',
          description: 'Statistics on each request consumption details, transparent pricing, no hidden costs, use with peace of mind'
        }
      },
      statistics: {
        models: 'Supported Models',
        users: 'Community Users',
        requests: 'Daily Requests',
        contributors: 'Contributors'
      },
      modelHot: {
        title: 'Model Popularity Ranking',
        description: 'Learn about users\' favorite AI models and choose the model that best suits your needs',
        distribution: 'Model Usage Distribution',
        distributionDesc: 'Model heat distribution map based on actual user usage',
        ranking: 'Popular Model Ranking',
        rankingDesc: 'Ranking of the most popular AI models'
      },
      callToAction: {
        title: 'Ready to start using Thor?',
        description: 'Register now and get free quota to start your AI development journey',
        register: 'Register for Free',
        docs: 'View Documentation'
      },
      projects: {
        title: 'Related Open Source Projects',
        aidotnet: {
          title: 'AIDotNet',
          description: 'AIDotNet Community is a community of AI developers, aiming to promote the development of AI technology and provide a better learning and communication platform for AI developers.',
          action: 'Learn More'
        },
        fastwiki: {
          title: 'FastWiki',
          description: 'An open-source project for intelligent knowledge base, which can be used to develop enterprise-level intelligent customer service management systems. Supporting multiple knowledge base formats, providing efficient retrieval and answer generation capabilities.',
          action: 'Learn More'
        }
      }
    },

    // Redeem Code Page
    redeemCode: {
      title: 'Redeem Code Management',
      name: 'Name',
      disabled: 'Disabled',
      quota: 'Quota',
      createdAt: 'Created At',
      redeemedUser: 'Redeemed User',
      redeemedTime: 'Redeemed Time',
      notRedeemed: 'Not Redeemed',
      createRedeemCode: 'Create Redeem Code',
      updateRedeemCode: 'Update Redeem Code',
      batchCreate: 'Batch Create',
      codeValue: 'Code Value',
      generateCode: 'Generate Code',
      validityPeriod: 'Validity Period',
      days: 'Days',
      limitQuantity: 'Limit Quantity',
      unlimited: 'Unlimited',
      customQuantity: 'Custom Quantity',
      createSuccess: 'Creation Successful',
      createFailed: 'Creation Failed'
    },

    // User Page
    user: {
      title: 'User Management',
      username: 'Username',
      email: 'Email',
      roles: 'Roles',
      status: 'Status',
      createdAt: 'Created At',
      lastLogin: 'Last Login',
      action: 'Action',
      addUser: 'Add User',
      editUser: 'Edit User',
      deleteUser: 'Delete User',
      resetPassword: 'Reset Password',
      enable: 'Enable',
      disable: 'Disable',
      enableSuccess: 'User enabled',
      disableSuccess: 'User disabled',
      deleteSuccess: 'User deleted',
      resetPasswordSuccess: 'Password reset',
      confirmDelete: 'Are you sure you want to delete this user?',
      confirmResetPassword: 'Are you sure you want to reset this user\'s password?'
    },

    // Token Management Page
    token: {
      title: 'Token Management',
      name: 'Name',
      token: 'Token Value',
      createdAt: 'Created At',
      expiredAt: 'Expired At',
      status: 'Status',
      createToken: 'Create Token',
      editToken: 'Edit Token',
      deleteToken: 'Delete Token',
      regenerateToken: 'Regenerate Token',
      copyToken: 'Copy Token',
      neverExpire: 'Never Expire',
      expired: 'Expired',
      active: 'Active',
      inactive: 'Inactive',
      remaining: 'Remaining'
    },

    // Model Management Page
    modelManager: {
      title: 'Model Management',
      modelName: 'Model Name',
      modelType: 'Model Type',
      baseUrl: 'Base URL',
      status: 'Status',
      apiKey: 'API Key',
      createModel: 'Create Model',
      editModel: 'Edit Model',
      deleteModel: 'Delete Model',
      testConnection: 'Test Connection',
      connectionSuccess: 'Connection Successful',
      connectionFailed: 'Connection Failed',
      confirmDelete: 'Are you sure you want to delete this model?'
    },

    // Product Page
    product: {
      title: 'Product Management',
      productName: 'Product Name',
      productType: 'Product Type',
      price: 'Price',
      discount: 'Discount',
      status: 'Status',
      createProduct: 'Create Product',
      editProduct: 'Edit Product',
      deleteProduct: 'Delete Product',
      confirmDelete: 'Are you sure you want to delete this product?'
    },

    // Channel Page
    channel: {
      title: 'Channel Management',
      channelName: 'Channel Name',
      channelType: 'Channel Type',
      apiKey: 'API Key',
      status: 'Status',
      createChannel: 'Create Channel',
      editChannel: 'Edit Channel',
      deleteChannel: 'Delete Channel',
      testConnection: 'Test Connection',
      connectionSuccess: 'Connection Successful',
      connectionFailed: 'Connection Failed',
      confirmDelete: 'Are you sure you want to delete this channel?'
    }
  }
};

export default enUS; 